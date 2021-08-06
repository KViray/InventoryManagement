using InventoryManagement.Enums;
using System;
using System.Text.Json.Serialization;

namespace InventoryManagement.Features.Items.Models
{
    public class AddItem
    {
        public string Code { get; set; }
        public string Brand { get; set; }
        public int InventoryId { get; set; }
    }
    public class GetItem
    {
        public string EmployeeId { get; set; }
        public bool? Availability { get; set; }
        public int? InventoryId { get; set; }
        public bool? OnLend { get; set; }
    }
    public class AddItems
    {
        [JsonPropertyName("id")]
        public Guid ItemId { get; set; }
        public string Code { get; set; }
        public string Brand { get; set; }
        public bool OnLend { get; set; }
        public bool Availability { get; set; }
        public string EmployeeId { get; set; }
        public int InventoryId { get; set; }
    }
    public class UpdateItem
    {
        public string Code { get; set; }
        public string Brand { get; set; }
        public int? InventoryId { get; set; }
    }
    public class ItemDetails
    {
        [JsonPropertyName("id")]
        public Guid ItemId { get; set; }
        public string Code { get; set; }
        public string Brand { get; set; }
        public bool OnLend { get; set; }
        public bool Availability { get; set; }
        public string EmployeeId { get; set; }
        public int InventoryId { get; set; }
    }
}
