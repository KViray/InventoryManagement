using InventoryManagement.Classes.Paging;
using InventoryManagement.Features.Employees.Models;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InventoryManagement.Features
{
    public class Functions
    {
        public string UploadImage(string id, FormFile file, string description, string imagepath)
        {
            if (File.Exists(imagepath))
            {
                File.Delete(imagepath);
            }
            string path = $"Resources/{description}/{id.ToLower()}";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filename = Path.GetFileName(file.FileName);
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", $"{description}", $"{id.ToLower()}", $"{filename}");
            using (FileStream fs = File.Create(filepath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            var newPath = string.Format("{0}/{1}/{2}", description, id.ToLower(), filename);
            return newPath;
        }

        public void UpdateDetails<T,F>(string id, T dataToUpdate, F newData)
        {
            foreach (var update in newData.GetType().GetProperties())
            {
                var newValue = update.GetValue(newData);
                var propertyInfo = dataToUpdate.GetType().GetProperties().Where(pi => pi.Name == update.Name).First();
                if (newValue != null && propertyInfo.Name == update.Name)
                {
                    if (newValue.GetType().Name == "FormFile")
                    {
                        var description = dataToUpdate.GetType().Name;
                        var image = dataToUpdate.GetType().GetProperties().Where(image => image.Name == "Image").First().GetValue(dataToUpdate).ToString();
                        var imagepath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", image);
                        propertyInfo.SetValue(dataToUpdate, UploadImage(id, (FormFile)newValue, description, imagepath));
                    }
                    else if (newValue.GetType().BaseType.Name == "Enum") 
                    {
                        propertyInfo.SetValue(dataToUpdate, $"{newValue}");
                    }
                    else
                    {
                        propertyInfo.SetValue(dataToUpdate, newValue);
                    }
                }
            }
        }
        public bool ParameterNullChecker<P>(P parameters)
        {
            var properties = parameters.GetType().GetProperties();
            bool isNull = true;
            foreach (var property in properties)
            {
                if(property.GetValue(parameters) != null)
                {
                    return false;
                }
            }
            return isNull;
        }
        public List<object> test(object parameters)
        {
            var properties = parameters.GetType().GetProperties();
            var p = new List<object>();
            foreach (var property in properties)
            {
                if (property.GetValue(parameters) != null)
                {
                    p.Add(property.Name);
                }
            }
            return p;
        }
        public PagedData MapToPagedData(dynamic pagedList)
        {
            return new PagedData
            {
                Result = pagedList.Result,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                Limit = pagedList.Limit,
                TotalCount = pagedList.TotalCount,
                HasPrevious = pagedList.HasPrevious,
                HasNext = pagedList.HasNext
            };
        }
        public EmployeeDetails<string> MaptoEmployeeDetails(Employee employeeDetails)
        {
            return new EmployeeDetails<string>
            {
                Image = employeeDetails.Image,
                EmployeeId = employeeDetails.EmployeeId,
                UserType = employeeDetails.UserType,
                FirstName = employeeDetails.FirstName,
                LastName = employeeDetails.LastName,
                MiddleName = employeeDetails.MiddleName,
                BirthDate = employeeDetails.BirthDate,
                Age = employeeDetails.Age,
                Sex = employeeDetails.Sex,
                Address = employeeDetails.Address,
                EmployedDate = employeeDetails.EmployedDate,
                Email = employeeDetails.Email
            };
        }
    }
}
