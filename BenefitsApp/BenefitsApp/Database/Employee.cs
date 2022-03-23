namespace BenefitsApp.Database
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal PayRate { get; set; }
        public List<Dependent> Dependents { get; set; }
    }
}
