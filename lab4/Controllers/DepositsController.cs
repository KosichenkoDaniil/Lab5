using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab4;
using lab4.ViewModels;
using System.Drawing.Printing;
using lab4.Infrastructure;
namespace lab4.Controllers
{
    public class DepositsController : Controller
    {
        private readonly BankDepositsContext _context;
        private readonly int pageSize = 10;

        public DepositsController(BankDepositsContext context)
        {
            _context = context;
        }

        // GET: Deposits
        public IActionResult Index(int page = 1)
        {
            var depositView = HttpContext.Session.Get<DepositViewModel>("Deposit");
            if (depositView == null)
            {
                depositView = new DepositViewModel();
            }

            IQueryable<Deposit> depositsDbContext = _context.Deposits.Include(o => o.Currency);
            depositsDbContext = Search(depositsDbContext, depositView.Name, depositView.Term, depositView.Mindepositamount, depositView.CurrencyName, depositView.Rate, depositView.Additionalconditions);
            var count = depositsDbContext.Count();
            depositsDbContext = depositsDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            DepositViewModel deposits = new DepositViewModel
            {
                deposits = depositsDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                Name = depositView.Name,
                Term = depositView.Term,
                Mindepositamount = depositView.Mindepositamount,
                CurrencyName = depositView.CurrencyName,
                Rate = depositView.Rate,
                Additionalconditions = depositView.Additionalconditions
            };
            return View(deposits);
        }
        [HttpPost]
        public IActionResult Index(DepositViewModel depositView)
        {
            HttpContext.Session.Set("Deposit", depositView);

            return RedirectToAction("Index");  
        }
        // GET: Deposits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }

            var deposit = await _context.Deposits
                .Include(d => d.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deposit == null)
            {
                return NotFound();
            }

            return View(deposit);
        }

        // GET: Deposits/Create
        public IActionResult Create()
        {
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name");
            return View();
        }

        // POST: Deposits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Term,Mindepositamount,CurrencyId,Rate,Additionalconditions")] Deposit deposit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deposit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", deposit.CurrencyId);
            return View(deposit);
        }

        // GET: Deposits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }

            var deposit = await _context.Deposits.FindAsync(id);
            if (deposit == null)
            {
                return NotFound();
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", deposit.CurrencyId);
            return View(deposit);
        }

        // POST: Deposits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Term,Mindepositamount,CurrencyId,Rate,Additionalconditions")] Deposit deposit)
        {
            if (id != deposit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deposit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepositExists(deposit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", deposit.CurrencyId);
            return View(deposit);
        }

        // GET: Deposits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }

            var deposit = await _context.Deposits
                .Include(d => d.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deposit == null)
            {
                return NotFound();
            }

            return View(deposit);
        }

        // POST: Deposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Deposits == null)
            {
                return Problem("Entity set 'BankDepositsContext.Deposits'  is null.");
            }
            var deposit = await _context.Deposits.FindAsync(id);
            if (deposit != null)
            {
                _context.Deposits.Remove(deposit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepositExists(int id)
        {
          return (_context.Deposits?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Deposit> Search(IQueryable<Deposit> deposits, string Name, double Term,
            decimal Mindepositamount, string CurrencyName, decimal Rate, string? Additionalconditions)
        {
            deposits = deposits.Where(o => (o.Name.Contains(Name ?? ""))
           && (o.Term == Term || Term == 0)
           && (o.Mindepositamount == Mindepositamount || Mindepositamount == 0)
           && (o.Currency.Name.Contains(CurrencyName ?? ""))
           && (o.Rate == Rate || Rate == 0)
           && (o.Additionalconditions.Contains(Additionalconditions ?? "")));

            return deposits;
        }
    }
}
