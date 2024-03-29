using InventoryWorld.Models;
using InventoryWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryWorld.Controllers
{
    public class BrandsController : Controller
    {
        private readonly AppDbContext _context;
        public BrandsController(AppDbContext context)
        {
           _context= context;
        }
        public IActionResult Index()
        {
            var brands = _context.Brands;
            return View(brands);
        }

        public IActionResult Create()
        {
           
            return View();
        }
       
        [HttpPost]
        public IActionResult Create(Brands brands)
        {
            if (ModelState.IsValid) {
                _context.Add(brands);
                var result =  _context.SaveChangesAsync().Result;
                if (result > 0)
                {
                  ViewBag.Alert=  AlertServices.ShowAlert(AlertServices.Alerts.Success, "New Brand Successfully Added");
                }
                else
                {
                    ViewBag.Alert = AlertServices.ShowAlert(AlertServices.Alerts.Danger, "SOmething Went Wrong Please Try Again Later");
                }
            }

            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Brands brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandsExistsAsync(brand.Id))
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
            return View(brand);
        }

        private  bool BrandsExistsAsync(int id)
        {
            var brand =  _context.Brands.Find(id);
            if (brand==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brands == null)
            {
                return NotFound();
            }

            return View(brands);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var brands = await _context.Brands.FindAsync(id);
            if (brands != null)
            {
                _context.Brands.Remove(brands);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
