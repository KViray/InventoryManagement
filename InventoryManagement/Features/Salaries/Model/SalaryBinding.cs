using System.Collections.Generic;

namespace InventoryManagement.Features.Salaries.Model
{
    public class SalaryBinding
    {
        public decimal BasicPay { get; set; }
        public decimal TotalDaysPresentDuringHoliday { get; set; }
        public decimal TotalMonthsintheCompanyInWholeYear { get; set; }
        public List<OverTimeDetails> OvertimePay { get; set; }
        public int TotalHoursOfUndertime { get; set; }
        public int TotalDaysOfAbsences { get; set; }
        public decimal Allowance { get; set; }
        public decimal SSS { get; set; }
        public decimal PhilHealth { get; set; }
        public decimal HDMF { get; set; }
        public decimal WithholdingTax { get; set; }
        public decimal Others { get; set; }
    }
    public class OverTimeDetails
    {
        public int HoursOfOvertime { get; set; }
        public decimal Rate { get; set; }
    }
}
