namespace BenefitsApp.Models
{
    public class EmployeePayrollInformation
    {
        public decimal PayPerPeriod { get; set; }
        public decimal BaseBenefitsCost { get; set; }
        public decimal BaseDiscount { get; set; }
        public int NumDependents { get; set; }
        public decimal DependentCost { get; set; }
        public decimal DependentDiscount { get; set; }
        public decimal NetPay => this.PayPerPeriod - this.BaseBenefitsCost + this.BaseDiscount - this.DependentCost + this.DependentDiscount;
    }
}
