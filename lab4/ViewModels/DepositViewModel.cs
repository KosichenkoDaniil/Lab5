namespace lab4.ViewModels
{
    public class DepositViewModel
    {
        public IEnumerable<Deposit> deposits { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }

        public double Term { get; set; }

        public decimal Mindepositamount { get; set; }

        public string CurrencyName { get; set; }

        public decimal Rate { get; set; }

        public string? Additionalconditions { get; set; }

    }

}

