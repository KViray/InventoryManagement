using System;

namespace InventoryManagement.Features.ItemHistories.Models
{
    public class ItemHistoryDetails
    {
        public Guid ItemId { get; set; }
        public string Message { get; set; }
        public DateTime? DateCreated { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    public class GetItemHistoryDetails
    {
        public Guid? ItemId { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
