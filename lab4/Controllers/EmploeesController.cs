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
    public class EmploeesController : Controller
    {
        private readonly BankDepositsContext _context;
        private readonly int pageSize = 10;

        public EmploeesController(BankDepositsContext context)
        {
            _context = context;
        }

        // GET: Emploees
        public IActionResult Index( int page = 1)
        {
            var emploeeView = HttpContext.Session.Get<EmploeeViewModel>("Emploee");
            if (emploeeView == null)
            {
                emploeeView = new EmploeeViewModel();
            }

            IQueryable<Emploee> emploeeDbContext = _context.Emploees;
            emploeeDbContext = Search(emploeeDbContext, emploeeView.Name, emploeeView.Surname, emploeeView.Middlename, emploeeView.Post, emploeeView.Dob);
            var count = emploeeDbContext.Count();
            emploeeDbContext = emploeeDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            EmploeeViewModel emploees = new EmploeeViewModel
            {
                emploees = emploeeDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                Name = emploeeView.Name,
                Surname = emploeeView.Surname,
                Middlename = emploeeView.Middlename,
                Post = emploeeView.Post,
                Dob = emploeeView.Dob   
            };
            return View(emploees);
        }

        [HttpPost]
        public IActionResult Index(EmploeeViewModel emploeeView)
        {
            HttpContext.Session.Set("Emploee", emploeeView);

            return RedirectToAction("Index");
        }

        // GET: Emploees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Emploees == null)
            {
                return NotFound();
            }

            var emploee = await _context.Emploees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emploee == null)
            {
                return NotFound();
            }

            return View(emploee);
        }

        // GET: Emploees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Emploees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Middlename,Post,Dob")] Emploee emploee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emploee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emploee);
        }

        // GET: Emploees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Emploees == null)
            {
                return NotFound();
            }

            var emploee = await _context.Emploees.FindAsync(id);
            if (emploee == null)
            {
                return NotFound();
            }
            return View(emploee);
        }

        // POST: Emploees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Middlename,Post,Dob")] Emploee emploee)
        {
            if (id != emploee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emploee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmploeeExists(emploee.Id))
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
            return View(emploee);
        }

        // GET: Emploees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Emploees == null)
            {
                return NotFound();
            }

            var emploee = await _context.Emploees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emploee == null)
            {
                return NotFound();
            }

            return View(emploee);
        }

        // POST: Emploees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Emploees == null)
            {
                return Problem("Entity set 'BankDeposits1Context.Emploees'  is null.");
            }
            var emploee = await _context.Emploees.FindAsync(id);
            if (emploee != null)
            {
                _context.Emploees.Remove(emploee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmploeeExists(int id)
        {
          return (_context.Emploees?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Emploee> Search(IQueryable<Emploee> emploees, string Name, string Surname, string Middlename, string Post, DateTime Dob)
        {
            emploees = emploees.Where(o => o.Name.Contains(Name ?? "")
            && (o.Surname.Contains(Surname ?? ""))
            && (o.Middlename.Contains(Middlename ?? ""))
            && (o.Post.Contains(Post ?? ""))
            && (o.Dob.Date == Dob || Dob == new DateTime() || Dob == null)    );

            return emploees;
        }
    }
}
