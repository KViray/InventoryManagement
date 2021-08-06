using InventoryManagement.Models;
using System;

namespace InventoryManagement.Classes
{
    public abstract class ItemClass
    {
        public abstract Item UpdateItem(Item item, Guid empId);
    }
    public class RegisterItem : ItemClass
    {
        public override Item UpdateItem(Item item, Guid empId)
        {
            item.Availability = false;
            item.EmployeeId = $"{empId}";
            return item;
        }
    }
    public class UnregisterItem : ItemClass
    {
        public override Item UpdateItem(Item item, Guid empId)
        {
            item.OnLend = false;
            item.Availability = true;
            item.EmployeeId = "";
            return item;
        }
    }

    public class LendItem : ItemClass
    {
        public override Item UpdateItem(Item item, Guid empId)
        {
            item.OnLend = true;
            item.EmployeeId = $"{empId}";
            return item;
        }
    }

    public class UnLendItem : ItemClass
    {
        public override Item UpdateItem(Item item, Guid empId)
        {
            item.OnLend = false;
            item.EmployeeId = $"{empId}";
            return item;
        }
    }
    
}
