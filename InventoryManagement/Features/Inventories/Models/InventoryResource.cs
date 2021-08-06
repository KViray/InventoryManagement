using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace InventoryManagement.Features.Inventories.Models
{
    public class AddInventories
    {
        public string InventoryName { get; set; }
        public string Image { get; set; }
    }
    public class UpdateInventory
    {
        public IFormFile Image { get; set; }
        public string InventoryName { get; set; }
    }
    public class InventoryDetails
    {
        [JsonPropertyName("id")]
        public int InventoryId { get; set; }
        public string Image { get; set; }
        public string InventoryName { get; set; }
    }
}
