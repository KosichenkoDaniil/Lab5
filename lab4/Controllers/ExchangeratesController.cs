using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab4;
using lab4.ViewModels;
using lab4.Infrastructure;

namespace lab4.Controllers
{
    public class ExchangeratesController : Controller
    {
        private readonly BankDepositsContext _context;
        private readonly int pageSize = 10;

        public ExchangeratesController(BankDepositsContext context)
        {
            _context = context;
        }

        // GET: Exchangerates
        
        public IActionResult Index( int page = 1)
        {
            var exchangeratesView = HttpContext.Session.Get<ExchangerateViewModel>("Exchangerate");
            if (exchangeratesView == null)
            {
                exchangeratesView = new ExchangerateViewModel();
            }

            IQueryable<Exchangerate> exchangeratesDbContext = _context.Exchangerates.Include(o => o.Currency);
            exchangeratesDbContext = Search(exchangeratesDbContext, exchangeratesView.Date, exchangeratesView.CurrencyName, exchangeratesView.Cost);
            var count = exchangeratesDbContext.Count();
            exchangeratesDbContext = exchangeratesDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            ExchangerateViewModel exchangerates = new ExchangerateViewModel
            {
                exchangerates = exchangeratesDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                Date = exchangeratesView.Date,
                CurrencyName = exchangeratesView.CurrencyName,
                Cost = exchangeratesView.Cost
            };
            return View(exchangerates);
        }

        [HttpPost]
        public IActionResult Index(ExchangerateViewModel exchangeratesView)
        {
            HttpContext.Session.Set("Exchangerate", exchangeratesView);

            return RedirectToAction("Index");
        }

        // GET: Exchangerates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Exchangerates == null)
            {
                return NotFound();
            }

            var exchangerate = await _context.Exchangerates
                .Include(e => e.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exchangerate == null)
            {
                return NotFound();
            }

            return View(exchangerate);
        }

        // GET: Exchangerates/Create
        public IActionResult Create()
        {
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name");
            return View();
        }

        // POST: Exchangerates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,CurrencyId,Cost")] Exchangerate exchangerate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exchangerate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", exchangerate.CurrencyId);
            return View(exchangerate);
        }

        // GET: Exchangerates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Exchangerates == null)
            {
                return NotFound();
            }

            var exchangerate = await _context.Exchangerates.FindAsync(id);
            if (exchangerate == null)
            {
                return NotFound();
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", exchangerate.CurrencyId);
            return View(exchangerate);
        }

        // POST: Exchangerates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,CurrencyId,Cost")] Exchangerate exchangerate)
        {
            if (id != exchangerate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exchangerate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExchangerateExists(exchangerate.Id))
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
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Name", exchangerate.CurrencyId);
            return View(exchangerate);
        }

        // GET: Exchangerates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Exchangerates == null)
            {
                return NotFound();
            }

            var exchangerate = await _context.Exchangerates
                .Include(e => e.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exchangerate == null)
            {
                return NotFound();
            }

            return View(exchangerate);
        }

        // POST: Exchangerates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Exchangerates == null)
            {
                return Problem("Entity set 'BankDepositsContext.Exchangerates'  is null.");
            }
            var exchangerate = await _context.Exchangerates.FindAsync(id);
            if (exchangerate != null)
            {
                _context.Exchangerates.Remove(exchangerate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExchangerateExists(int id)
        {
          return (_context.Exchangerates?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Exchangerate> Search(IQueryable<Exchangerate> exchangerates, DateTime Date, string CurrencyName,
            decimal Cost)
        {
            exchangerates = exchangerates.Where(o => (o.Date == Date || Date == new DateTime())
           && (o.Currency.Name.Contains(CurrencyName ?? ""))
           && (o.Cost == Cost || Cost == 0));

            return exchangerates;
        }
    }
}
