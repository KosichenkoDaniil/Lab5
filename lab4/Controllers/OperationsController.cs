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
    public class OperationsController : Controller
    {
        private readonly BankDepositsContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице

        public OperationsController(BankDepositsContext context)
        {
            _context = context;
        }

        // GET: Operations
        
        public  IActionResult Index( int page = 1)
        {
            var operationView = HttpContext.Session.Get<OperationViewModel>("Operation");
            if (operationView == null)
            {
                operationView =new OperationViewModel();
            }

            IQueryable<Operation> operationsDbContext = _context.Operations.Include(o => o.Investors).Include(o => o.Emploee).Include(o => o.Deposit);
            operationsDbContext = Search(operationsDbContext, operationView.InvestorsName, operationView.Depositdate, operationView.Returndate, operationView.Deposit
    , operationView.Depositamount, operationView.Refundamount, operationView.Returnstamp, operationView.EmploeeName);
            var count = operationsDbContext.Count();
            operationsDbContext = operationsDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            OperationViewModel operations = new OperationViewModel
            {
                operations = operationsDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                InvestorsName = operationView.InvestorsName,
                Depositdate = operationView.Depositdate,
                Returndate = operationView.Returndate,
                Deposit = operationView.Deposit,
                Depositamount = operationView.Depositamount,
                Refundamount = operationView.Refundamount,
                Returnstamp = operationView.Returnstamp,
                EmploeeName = operationView.EmploeeName
            };
            return View(operations);
        }

        [HttpPost]
        public IActionResult Index(OperationViewModel operationView)
        {
            HttpContext.Session.Set("Operation", operationView);

            return RedirectToAction("Index");
        }

        // GET: Operations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Operations == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Deposit)
                .Include(o => o.Emploee)
                .Include(o => o.Investors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // GET: Operations/Create
        public IActionResult Create()
        {
            ViewData["DepositId"] = new SelectList(_context.Deposits, "Id", "Name");
            ViewData["EmploeeId"] = new SelectList(_context.Emploees, "Id", "Id");
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id");
            return View();
        }

        // POST: Operations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Investorsid,Depositdate,Returndate,Depositid,Depositamount,Refundamount,Returnstamp,Emploeeid")] Operation operation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(operation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepositId"] = new SelectList(_context.Deposits, "Id", "Name", operation.DepositId);
            ViewData["EmploeeId"] = new SelectList(_context.Emploees, "Id", "Id", operation.EmploeeId);
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id", operation.InvestorId);
            return View(operation);
        }

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Operations == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }
            ViewData["DepositId"] = new SelectList(_context.Deposits, "Id", "Name", operation.DepositId);
            ViewData["EmploeeId"] = new SelectList(_context.Emploees, "Id", "Id", operation.EmploeeId);
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id", operation.InvestorId);
            return View(operation);
        }

        // POST: Operations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvestorId,Depositdate,Returndate,DepositId,Depositamount,Refundamount,Returnstamp,EmploeeId")] Operation operation)
        {
            if (id != operation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(operation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationExists(operation.Id))
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
            ViewData["DepositId"] = new SelectList(_context.Deposits, "Id", "Name", operation.DepositId);
            ViewData["EmploeeId"] = new SelectList(_context.Emploees, "Id", "Id", operation.EmploeeId);
            ViewData["InvestorId"] = new SelectList(_context.Investors, "Id", "Id", operation.InvestorId);
            return View(operation);
        }

        // GET: Operations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Operations == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Deposit)
                .Include(o => o.Emploee)
                .Include(o => o.Investors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Operations == null)
            {
                return Problem("Entity set 'BankDeposits1Context.Operations'  is null.");
            }
            var operation = await _context.Operations.FindAsync(id);
            if (operation != null)
            {
                _context.Operations.Remove(operation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationExists(int id)
        {
          return (_context.Operations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Operation> Search(IQueryable<Operation> operations, string InvestorsName,  DateTime Depositdate,
            DateTime Returndate, string Deposit, decimal Depositamount, decimal Refundamount,
            bool Returnstamp, string EmploeeName)
        {
             operations = operations.Where(o => o.Investors.Name.Contains(InvestorsName ?? "")
            && (o.Depositdate == Depositdate || Depositdate == new DateTime())
            && (o.Returndate == Returndate || Returndate == new DateTime())
            && (o.Deposit.Name.Contains(Deposit ?? ""))
            && (o.Depositamount == Depositamount || Depositamount ==0)
            && (o.Refundamount == Refundamount || Refundamount == 0)
            && (o.Returnstamp == Returnstamp)
            && (o.Emploee.Name.Contains(EmploeeName ?? "")));

            return operations;
        }
    }
}
