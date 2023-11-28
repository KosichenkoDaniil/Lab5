using System;
using System.Collections.Generic;

namespace lab4;

public partial class Operation
{
    public int Id { get; set; }

    public int InvestorId { get; set; }

    public DateTime Depositdate { get; set; }

    public DateTime Returndate { get; set; }

    public int DepositId { get; set; }

    public decimal Depositamount { get; set; }

    public decimal Refundamount { get; set; }

    public bool Returnstamp { get; set; }

    public int EmploeeId { get; set; }

    public virtual Deposit? Deposit { get; set; } = null!;

    public virtual Emploee? Emploee { get; set; } = null!;

    public virtual Investor? Investors { get; set; } = null!;
}
