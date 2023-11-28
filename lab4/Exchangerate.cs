using System;
using System.Collections.Generic;

namespace lab4;

public partial class Exchangerate
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int CurrencyId { get; set; }

    public decimal Cost { get; set; }

    public virtual Currency? Currency { get; set; } 
}
