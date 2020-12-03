using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AngularApi.Models;

namespace AngularApi.DataBase
{
    public class CashDBModelsController : Controller
    {
        private readonly CashDBContext _context;

        public CashDBModelsController(CashDBContext context)
        {
            _context = context;
        }

        // GET: CashDBModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.cashDBModels.ToListAsync());
        }

        // GET: CashDBModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashDBModel = await _context.cashDBModels
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cashDBModel == null)
            {
                return NotFound();
            }

            return View(cashDBModel);
        }

        // GET: CashDBModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CashDBModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Code,BidPrice,AskPrice,Data")] CurrencyDBModel cashDBModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cashDBModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cashDBModel);
        }

        // GET: CashDBModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashDBModel = await _context.cashDBModels.FindAsync(id);
            if (cashDBModel == null)
            {
                return NotFound();
            }
            return View(cashDBModel);
        }

        // POST: CashDBModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Code,BidPrice,AskPrice,Data")] CurrencyDBModel cashDBModel)
        {
            if (id != cashDBModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cashDBModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashDBModelExists(cashDBModel.ID))
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
            return View(cashDBModel);
        }

        // GET: CashDBModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashDBModel = await _context.cashDBModels
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cashDBModel == null)
            {
                return NotFound();
            }

            return View(cashDBModel);
        }

        // POST: CashDBModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cashDBModel = await _context.cashDBModels.FindAsync(id);
            _context.cashDBModels.Remove(cashDBModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CashDBModelExists(int id)
        {
            return _context.cashDBModels.Any(e => e.ID == id);
        }
    }
}
