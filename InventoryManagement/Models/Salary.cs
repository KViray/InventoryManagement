using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Salary : Common
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string EmployeeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal HolidayPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ThirteenthMonthPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OvertimePay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Absences { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnderTime { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Allowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SSS { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PhilHealth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal HDMF { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal WithholdingTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Others { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSalary { get; set; }
    }
}
