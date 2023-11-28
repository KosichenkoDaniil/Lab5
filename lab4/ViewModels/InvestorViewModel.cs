namespace lab4.ViewModels
{
    public class InvestorViewModel
    {
        
        

        public IEnumerable<Investor> investors { get; set; }
        public PageViewModel PageViewModel { get; set; }

        public string Name { get; set; } 

        public string Surname { get; set; }

        public string Middlename { get; set; }

        public string Address { get; set; } 

        public string Phonenumber { get; set; } 

        public string PassportId { get; set; } 


    }


}
