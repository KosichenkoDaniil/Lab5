using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab4;
using lab4.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using lab4.Infrastructure;

namespace lab4.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly BankDepositsContext _context;
        private readonly int pageSize = 10;

        public CurrenciesController(BankDepositsContext context)
        {
            _context = context;
        }

        // GET: Currencies
        public IActionResult Index( int page = 1)
        {
            var currencyView = HttpContext.Session.Get<CurrencyViewModel>("Currency");
            if (currencyView == null)
            {
                currencyView = new CurrencyViewModel();
            }

            IQueryable<Currency> currencyDbContext = _context.Currencies;
            currencyDbContext = Search(currencyDbContext, currencyView.Name, currencyView.Country);
            var count = currencyDbContext.Count();
            currencyDbContext = currencyDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            CurrencyViewModel currencies= new CurrencyViewModel
            {
                currencies = currencyDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                Name = currencyView.Name,
                Country = currencyView.Country
            };
            return View(currencies);
        }

        [HttpPost]
        public IActionResult Index(CurrencyViewModel currencyView)
        {
            HttpContext.Session.Set("Currency", currencyView);

            return RedirectToAction("Index");
        }

        // GET: Currencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Currencies == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // GET: Currencies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Currencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                _context.Add(currency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(currency);
        }

        // GET: Currencies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Currencies == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencies.FindAsync(id);
            if (currency == null)
            {
                return NotFound();
            }
            return View(currency);
        }

        // POST: Currencies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country")] Currency currency)
        {
            if (id != currency.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currency);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrencyExists(currency.Id))
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
            return View(currency);
        }

        // GET: Currencies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Currencies == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Currencies == null)
            {
                return Problem("Entity set 'BankDepositsContext.Currencies'  is null.");
            }
            var currency = await _context.Currencies.FindAsync(id);
            if (currency != null)
            {
                _context.Currencies.Remove(currency);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrencyExists(int id)
        {
          return (_context.Currencies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Currency> Search(IQueryable<Currency> currencies, string Name, string Country)
        {
            currencies = currencies.Where(o => o.Name.Contains(Name ?? "")
            && (o.Country.Contains(Country ?? "")));

            return currencies;
        }
    }
}
