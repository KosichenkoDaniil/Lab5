namespace lab4.ViewModels
{
    public class CurrencyViewModel
    {
        public IEnumerable<Currency> currencies { get; set; }
        public PageViewModel PageViewModel { get; set; }

        public string Name { get; set; } 

        public string Country { get; set; } 


    }


}
