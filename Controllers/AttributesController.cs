using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryWorld.Models;

namespace InventoryWorld.Controllers
{
    public class AttributesController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        // GET: Attributes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Attributes.ToListAsync());
        }

        // GET: Attributes/Details/5
        public async Task<IActionResult> AddValues(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributes = await _context.Attributes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attributes == null)
            {
                return NotFound();
            }


            return View(attributes);
        }

        [HttpPost]
        public async Task<IActionResult> AddValues(AddAttValueModel model)
        {

            if (model.Id == null)
            {
                return NotFound();
            }

            var attributes = await _context.Attributes
                .FirstOrDefaultAsync(m => m.Id == model.Id);
            if (attributes == null)
            {
                return NotFound();
            }

         else

            if (!String.IsNullOrEmpty(model.Values))
            {
                attributes.AddValue(model.Values);
               // attributes.Value.Add(model.Values);
            }
           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attributes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributesExists(attributes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json("Value Successfully Added");
            }
            return Json("Unable to Save Data");
        }


        [HttpPost]
        public JsonResult EditValues(UpdateAttModel model)
        {
            if (model.Id == null)
            {
                return Json("Id not found");
            }

            var attributes =  _context.Attributes
                .FirstOrDefault(m => m.Id == model.Id);
            if (attributes is not null)
            {
               attributes.EditValue(model.OldValue, model.NewValue);

                _context.Update(attributes);
            _context.SaveChanges();

                return Json("Attribute Updated Successfully");
            }
            return Json("Attribute not found");

        }

        public JsonResult RemoveValues(Delete model)
        {
            if (model.Id == null)
            {
                return Json("Id not found");
            }

            var attributes = _context.Attributes
                .FirstOrDefault(m => m.Id == model.Id);

            if(attributes is not null)
            {
                attributes.RemoveValue(model.Data);
                _context.Update(attributes);
                _context.SaveChanges();
                return Json("Attribute Removed Successfully");

            }
            return Json("Attribute not found");
        }
        // GET: Attributes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attributes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AttributeName,Value,Status")] Attributes attributes)
        {
          //  attributes.ProductAttributes = new List<ProductAttribute>();
            if (ModelState.IsValid)
            {
                _context.Add(attributes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attributes);
        }

        // GET: Attributes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributes = await _context.Attributes.FindAsync(id);
            if (attributes == null)
            {
                return NotFound();
            }
            return View(attributes);
        }

        // POST: Attributes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AttributeName,Value,Status")] Attributes attributes)
        {
            if (id != attributes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attributes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributesExists(attributes.Id))
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
            return View(attributes);
        }

        // GET: Attributes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributes = await _context.Attributes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attributes == null)
            {
                return NotFound();
            }

            return View(attributes);
        }

        // POST: Attributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attributes = await _context.Attributes.FindAsync(id);
            if (attributes != null)
            {
                _context.Attributes.Remove(attributes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttributesExists(int id)
        {
            return _context.Attributes.Any(e => e.Id == id);
        }
    }
}
