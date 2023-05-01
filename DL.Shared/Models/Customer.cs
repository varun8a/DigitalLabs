namespace DL.Shared.Models
{
    //Customer Model
    public class Customer
    {
        public long CustomerSSN { get; set; }
        public string FullName { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal EquityAmount { get; set; }
        public decimal SalaryAmount { get; set; }
    }
}