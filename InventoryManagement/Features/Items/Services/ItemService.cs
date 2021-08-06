using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InventoryManagement.Classes;
using InventoryManagement.Classes.Paging;
using InventoryManagement.Context;
using InventoryManagement.Features.Items.Models;
using InventoryManagement.Models;
using LinqKit;
using Microsoft.AspNetCore.Http;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Items.Services
{
    public class ItemService : IItemService
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly Functions _functions;
        private readonly Cloudinary _cloudinary;
        public ItemService(InventoryDbContext inventoryDbContext, Functions functions, Cloudinary cloudinary)
        {
            _inventoryDbContext = inventoryDbContext;
            _functions = functions;
            _cloudinary = cloudinary;
        }

        public async Task<ItemDetails> AddItem(AddItem addItem, string userId)
        {
            var employeeName = _inventoryDbContext.Employee.Where(emp => $"{emp.Id}" == userId).FirstOrDefault();
            var newItemGuid = Guid.NewGuid();
            var message = string.Format("{0} {1} added a new item {2}", employeeName.FirstName, employeeName.LastName, userId);
            var newItem = new Item
            {
                ItemId = newItemGuid,
                Code = addItem.Code,
                Brand = addItem.Brand,
                OnLend = false,
                Availability = true,
                EmployeeId = "",
                InventoryId = addItem.InventoryId,
                IsDeleted = 0
            };
            await GenerateQRCode($"{newItemGuid}");
            var newItemHistory = Items(typeof(ItemHistoryClass))["AddItemHistory"].AddOrUpdateItemHistory(newItem, message, $"{userId}");
            _inventoryDbContext.Item.Add(newItem);
            _inventoryDbContext.ItemHistory.Add(newItemHistory);
            _inventoryDbContext.SaveChanges();
            return await Task.FromResult(MapToItemDetails(newItem));
        }

        public async Task<IEnumerable<ItemDetails>> AddItems(AddItems[] itemDetails)
        {
            var listOfItems = new List<ItemDetails>();
            foreach(var item in itemDetails)
            {
                var newItem = new Item
                {
                    ItemId = item.ItemId,
                    Code = item.Code,
                    Brand = item.Brand,
                    OnLend = item.OnLend,
                    Availability = item.Availability,
                    EmployeeId = item.EmployeeId,
                    InventoryId = item.InventoryId,
                    IsDeleted = 0
                };
                await GenerateQRCode($"{item.ItemId}");
                listOfItems.Add(MapToItemDetails(newItem));
                _inventoryDbContext.Item.Add(newItem);
                _inventoryDbContext.SaveChanges();
            }
            return await Task.FromResult(listOfItems);
        }

        public async Task DeleteItem(string id)
        {
            var item = _inventoryDbContext.Item.Where(item => $"{item.ItemId}" == id).SingleOrDefault();
            item.IsDeleted = 1;
            await Task.FromResult(_inventoryDbContext.SaveChanges());
        }

        public async Task<ItemDetails> GetItemById(string id)
        {
            var getItem = _inventoryDbContext.Item.Where(item => item.IsDeleted == 0 && $"{item.ItemId}" == id).FirstOrDefault();
            if (getItem == null) return null;

            return await Task.FromResult(MapToItemDetails(getItem));
        }

        public async Task<PagedData> GetItems(GetItem getItem, Paging paging)
        {

            IEnumerable<ItemDetails> getAllItems;

            if (_functions.ParameterNullChecker(getItem))
            {
                getAllItems = _inventoryDbContext.Item.Where(items => items.IsDeleted == 0).Select(MapToItemDetails);
            }
            else
            {
                getAllItems = _inventoryDbContext.Item.Where(items => ((getItem.EmployeeId == null || items.EmployeeId == getItem.EmployeeId)
                                                                   && (getItem.Availability == null || items.Availability == getItem.Availability)
                                                                   && (getItem.InventoryId == null || items.InventoryId == getItem.InventoryId)
                                                                   && (getItem.OnLend == null || items.OnLend == getItem.OnLend))
                                                                   && items.IsDeleted == 0).Select(MapToItemDetails);
            }
            var rt = new List<dynamic>();
            var x = _inventoryDbContext.Item.Select(item => item.GetType().GetProperties());

            foreach(var sd in x)
            {
                rt.Add(sd);
            }
            if (_functions.ParameterNullChecker(paging))
            {
                return await Task.FromResult(_functions.MapToPagedData(PagedList<ItemDetails>.ToPagedList(getAllItems, 1, getAllItems.Count())));
            }
            return await Task.FromResult(_functions.MapToPagedData(PagedList<ItemDetails>.ToPagedList(getAllItems, paging.Page.Value, paging.Limit.Value)));

        }
        
        public async Task<Item> UpdateItem(string id, UpdateItem updateItem, string userId)
        {
            var item = _inventoryDbContext.Item.Where(item => item.IsDeleted == 0 && $"{item.ItemId}" == id).FirstOrDefault();
            var employeeName = _inventoryDbContext.Employee.Where(emp => $"{emp.Id}" == userId).FirstOrDefault();
            var message = string.Format("{0} {1} updated {2}", employeeName.FirstName, employeeName.LastName, item.Code);
            if (item == null) return null;

            _functions.UpdateDetails(id, item, updateItem);
            var newItemHistory = Items(typeof(ItemHistoryClass))["UpdateItemHistory"].AddOrUpdateItemHistory(item, message, $"{userId}");
            _inventoryDbContext.ItemHistory.Add(newItemHistory);
            _inventoryDbContext.SaveChanges();
            return await Task.FromResult(item);
        }

        public async Task<IEnumerable<ItemDetails>> UpdateItemStatus(Guid employeeId, Guid[] itemIds, string type)
        {
            var listOfUpdatedItems = new List<ItemDetails>();
            foreach (var ids in itemIds)
            {
                var items = _inventoryDbContext.Item.Where(item => item.ItemId == ids && item.IsDeleted == 0).FirstOrDefault();
                var employeeName = _inventoryDbContext.Employee.Where(emp => emp.Id == employeeId).FirstOrDefault();
                var message = string.Format("{0} {1} {2}ed {3}", employeeName.FirstName, employeeName.LastName, type.ToLower(), items.Code);
                if (items != null)
                {
                    Items(typeof(ItemClass))[type].UpdateItem(items, employeeId);
                    var newItemHistory = Items(typeof(ItemHistoryClass))["UpdateItemHistory"].AddOrUpdateItemHistory(items, message, $"{employeeId}");
                    _inventoryDbContext.ItemHistory.Add(newItemHistory);
                    _inventoryDbContext.SaveChanges();
                    listOfUpdatedItems.Add(MapToItemDetails(items));
                }
            }
            return await Task.FromResult(listOfUpdatedItems);
        }

        private Dictionary<string, dynamic> Items(Type classType)
        {
            Dictionary<string, dynamic> update = null;
            foreach (var type in classType.Assembly.GetTypes().Where(t => t.IsSubclassOf(classType) && !t.IsAbstract))
            {
                update.Add(type.Name, Activator.CreateInstance(type));
            }
            return update;
        }

        private ItemDetails MapToItemDetails(Item item)
        {
            return new ItemDetails
            {
                ItemId = item.ItemId,
                Code = item.Code,
                Brand = item.Brand,
                OnLend = item.OnLend,
                Availability = item.Availability,
                EmployeeId = item.EmployeeId == null ? "" : item.EmployeeId.ToLower(),
                InventoryId = item.InventoryId
            };
        }

        private async Task GenerateQRCode(string itemId)
        {
            var qRCode = new QRCodeGenerator().CreateQrCode(itemId, QRCodeGenerator.ECCLevel.Q);
            var qr = new QRCode(qRCode);
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Others", "yoonet-social.jpg");
            var bitMap = qr.GetGraphic(20, darkColor: Color.FromArgb(155, 188, 242), lightColor: Color.FromArgb(255, 255, 255), icon: (Bitmap)Bitmap.FromFile(directory));
            using MemoryStream stream = new MemoryStream();
            bitMap.Save(stream, ImageFormat.Png);
            var file = new FormFile(stream, 0, stream.Length, "QRCode", $"{ itemId }.png");
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "QRCodes",
                PublicId = itemId,
            };
            await Task.WhenAll(_cloudinary.UploadAsync(uploadParams));
        }
    }
}
