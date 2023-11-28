namespace lab4.ViewModels
{
    public class EmploeeViewModel
    {
        
            public IEnumerable<Emploee> emploees { get; set; }
            public PageViewModel PageViewModel { get; set; }

            public string Name { get; set; } 

            public string Surname { get; set; } 

            public string Middlename { get; set; } 

            public string Post { get; set; } 

            public DateTime Dob { get; set; }
    }
 
}
