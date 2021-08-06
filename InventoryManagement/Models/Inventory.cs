using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Inventory : Common
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        
        public int InventoryId { get; set; }
        public string Image { get; set; }
        public string InventoryName { get; set; }
        
    }
}
