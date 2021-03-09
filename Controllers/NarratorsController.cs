using AfrikanEchoes.Entities;
using AfrikanEchoes.ViewModels.Narrators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Controllers
{
    [Authorize]
    public class NarratorsController : Controller
    {
        private readonly AfrikanEchoesDbContext _context;

        public NarratorsController(AfrikanEchoesDbContext context)
        {
            _context = context;
        }

        // GET: Narrators
        public async Task<IActionResult> Index()
        {
            return View(await _context.Narrators.ToListAsync());
        }

        // GET: Narrators/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var narrator = await _context.Narrators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (narrator == null)
            {
                return NotFound();
            }

            return View(narrator);
        }

        // GET: Narrators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Narrators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Address,Email,PhoneNumber")] NarratorCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Narrator narrator = new Narrator
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Address = viewModel.Address,
                    Email = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                };
                _context.Add(narrator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Narrators/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var narrator = await _context.Narrators.FindAsync(id);
            if (narrator == null)
            {
                return NotFound();
            }
            NarratorEditViewModel viewModel = new NarratorEditViewModel
            {
                Id = narrator.Id,
                FirstName = narrator.FirstName,
                LastName = narrator.LastName,
                Address = narrator.Address,
                Email = narrator.Email,
                PhoneNumber = narrator.PhoneNumber,
            };
            return View(viewModel);
        }

        // POST: Narrators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,Address,Email,PhoneNumber")] NarratorEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Narrator narrator = await _context.Narrators.FindAsync(id);

                    narrator.FirstName = model.FirstName;
                    narrator.LastName = model.LastName;
                    narrator.Address = model.Address;
                    narrator.Email = model.Email;
                    narrator.PhoneNumber = model.PhoneNumber;
                    narrator.ModifiedAt = DateTime.Now;

                    _context.Update(narrator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NarratorExists(model.Id))
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
            return View(model);
        }

        // GET: Narrators/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var narrator = await _context.Narrators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (narrator == null)
            {
                return NotFound();
            }

            return View(narrator);
        }

        // POST: Narrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var narrator = await _context.Narrators.FindAsync(id);
            _context.Narrators.Remove(narrator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NarratorExists(long id)
        {
            return _context.Narrators.Any(e => e.Id == id);
        }
    }
}
