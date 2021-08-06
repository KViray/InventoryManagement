namespace InventoryManagement.Features.Salaries.Model
{
    public class SalaryResource
    {
        public decimal BasicPay { get; set; }
        public decimal HolidayPay { get; set; }
        public decimal ThirteenthMonthPay { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal Absences { get; set; }
        public decimal UnderTime { get; set; }
        public decimal Allowance { get; set; }
        public decimal SSS { get; set; }
        public decimal PhilHealth { get; set; }
        public decimal HDMF { get; set; }
        public decimal WithholdingTax { get; set; }
        public decimal Others { get; set; }
        public decimal TotalSalary { get; set; }
    }
}
