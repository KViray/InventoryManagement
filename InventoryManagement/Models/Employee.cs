using InventoryManagement.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Employee : Common
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string EmployeeId { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public DateTime EmployedDate { get; set; }
        public string Email { get; set; }

    }
}
