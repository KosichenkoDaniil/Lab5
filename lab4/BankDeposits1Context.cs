    using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lab4;

public partial class BankDepositsContext : IdentityDbContext
{
    public BankDepositsContext()
    {
    }

    public BankDepositsContext(DbContextOptions<BankDepositsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<Emploee> Emploees { get; set; }

    public virtual DbSet<Exchangerate> Exchangerates { get; set; }

    public virtual DbSet<Investor> Investors { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }
}
