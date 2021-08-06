using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryManagement.Models
{
    public class Item: Common
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

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
    