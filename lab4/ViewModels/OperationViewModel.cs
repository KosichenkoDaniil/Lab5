namespace lab4.ViewModels
{
    public class OperationViewModel
    {
        public IEnumerable<Operation> operations { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string InvestorsName { get; set; }

        public DateTime Depositdate { get; set; }

        public DateTime Returndate { get; set; }

        public string Deposit { get; set; }

        public decimal Depositamount { get; set; }

        public decimal Refundamount { get; set; }

        public bool Returnstamp { get; set; }

        public string EmploeeName { get; set; }
    }
}
