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
    public class InvestorsController : Controller
    {
        private readonly BankDepositsContext _context;
        private readonly int pageSize = 10;

        public InvestorsController(BankDepositsContext context)
        {
            _context = context;
        }

        // GET: Investors
        
        public IActionResult Index( int page = 1)
        {
            var investorView = HttpContext.Session.Get<InvestorViewModel>("Investor");
            if (investorView == null)
            {
                investorView = new InvestorViewModel();
            }

            IQueryable<Investor> investorDbContext = _context.Investors;
            investorDbContext = Search(investorDbContext, investorView.Name, investorView.Surname, investorView.Middlename, investorView.Address, investorView.Phonenumber, investorView.PassportId);
            var count = investorDbContext.Count();
            investorDbContext = investorDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            InvestorViewModel emploees = new InvestorViewModel
            {
                investors = investorDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                Name = investorView.Name,
                Surname = investorView.Surname,
                Middlename = investorView.Middlename,
                Address = investorView.Address,
                Phonenumber = investorView.Phonenumber,
                PassportId = investorView.PassportId
            };
            return View(emploees);
        }

        [HttpPost]
        public IActionResult Index(InvestorViewModel investorView)
        {
            HttpContext.Session.Set("Investor", investorView);

            return RedirectToAction("Index");
        }

        // GET: Investors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Investors == null)
            {
                return NotFound();
            }

            var investor = await _context.Investors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investor == null)
            {
                return NotFound();
            }

            return View(investor);
        }

        // GET: Investors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Investors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Middlename,Address,Phonenumber,PassportId")] Investor investor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(investor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(investor);
        }

        // GET: Investors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Investors == null)
            {
                return NotFound();
            }

            var investor = await _context.Investors.FindAsync(id);
            if (investor == null)
            {
                return NotFound();
            }
            return View(investor);
        }

        // POST: Investors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Middlename,Address,Phonenumber,PassportId")] Investor investor)
        {
            if (id != investor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(investor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestorExists(investor.Id))
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
            return View(investor);
        }

        // GET: Investors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Investors == null)
            {
                return NotFound();
            }

            var investor = await _context.Investors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investor == null)
            {
                return NotFound();
            }

            return View(investor);
        }

        // POST: Investors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Investors == null)
            {
                return Problem("Entity set 'BankDeposits1Context.Investors'  is null.");
            }
            var investor = await _context.Investors.FindAsync(id);
            if (investor != null)
            {
                _context.Investors.Remove(investor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvestorExists(int id)
        {
          return (_context.Investors?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Investor> Search(IQueryable<Investor> investors, string Name, string Surname, string Middlename, string Address, string Phonenumber, string PassportId )
        {
            investors = investors.Where(o => o.Name.Contains(Name ?? "")
            && (o.Surname.Contains(Surname ?? ""))
            && (o.Middlename.Contains(Middlename ?? ""))
            && (o.Address.Contains(Address ?? ""))
            && (o.Phonenumber.Contains(Phonenumber ?? ""))
            && (o.PassportId.Contains(PassportId ?? "")));

            return investors;
        }
    }
}
