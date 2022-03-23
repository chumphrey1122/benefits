namespace BenefitsApp.Models
{
    /// <summary>
    /// Information about a user's pay and payroll deductions.
    /// 
    /// TODO: Do we need all of these properties? Not all of them are explicitly being used by the front end.
    /// </summary>
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
