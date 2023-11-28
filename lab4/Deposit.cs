using System;
using System.Collections.Generic;

namespace lab4;

public partial class Deposit
{
    public int Id { get; set; }

    public string Name { get; set; }

    public double Term { get; set; }

    public decimal Mindepositamount { get; set; }

    public int CurrencyId { get; set; }

    public decimal Rate { get; set; }

    public string? Additionalconditions { get; set; }

    public virtual Currency? Currency { get; set; }

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();
}
