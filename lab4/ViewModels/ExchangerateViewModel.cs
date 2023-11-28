namespace lab4.ViewModels
{
    public class ExchangerateViewModel
    {
        public IEnumerable<Exchangerate> exchangerates { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public DateTime Date { get; set; }

        public string CurrencyName { get; set; }

        public decimal Cost { get; set; }

    }



}
