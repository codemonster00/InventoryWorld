using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryWorld.Models;
using System.Drawing;
using System.Diagnostics;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;


namespace InventoryWorld.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Products.Include(p => p.Store).Include(p=>p.Brand);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributes = _context.ProductAttributes.Include(x=>x.Attribute).Where(p => p.ProductId == id);
            ViewBag.attributes = attributes;
            var products = await _context.Products
                .Include(p => p.Store).Include(x=>x.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["StoresName"] = new SelectList(_context.Stores, "StoresName", "StoresName","Please Select Store");
            ViewData["BrandsName"] = new SelectList(_context.Brands, "BrandName", "BrandName", "Please Select Store");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Sku,Quantity,BrandsName,StoresName,Availability")] Products products,string ?Storename,string ?Brandname, IFormCollection form)
        {

         

            var brand= _context.Brands.FirstOrDefault(x=>x.BrandName == Brandname);
            if(brand is not null)
            {
                products.Brand= brand;
               // products.BrandsName = brand.Id;
            }

            var store = _context.Stores.FirstOrDefault(x => x.StoresName == Storename);
            if (store is not null)
            {
                products.Store = store;
               // products.StoresName = store.Id;
            }

            _context.Update(products);
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();

                var attList = new List<string>();
                var attributes = _context.Attributes.ToArray();
                foreach (var attr in attributes)
                {
                    attList.Add(form["dynamic" + attr.AttributeName].ToString());
                    _context.ProductAttributes.Add(new ProductAttribute { Attribute = attr, Value = form["dynamic" + attr.AttributeName].ToString() ,Product=products });
                   
                }
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            ViewData["StoresName"] = new SelectList(_context.Stores, "StoresName", "StoresName");
            ViewData["BrandsName"] = new SelectList(_context.Brands, "BrandName", "BrandName", "Please Select Store");
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.id = "10";

            var products = await _context.Products.Include(s=>s.Store).Include(s=>s.Brand).FirstOrDefaultAsync(x=>x.Id==id);
            var prodatt = _context.ProductAttributes.Where(x=>x.ProductId==products.Id).Include(x=>x.Attribute);
            if (prodatt != null)
            {
                var atttvalue = new List<Dictionary<string, string>>();
                foreach (var attr in prodatt)
                {
                    atttvalue.Add(new Dictionary<string, string> { { attr.Attribute.AttributeName,attr.Value } });
                }
                ViewBag.atttvalue = atttvalue;
            }
            if (products == null)
            {
                return NotFound();
            }
          
            ViewData["StoresName"] = new SelectList(_context.Stores, "StoresName", "StoresName");
            ViewData["BrandsName"] = new SelectList(_context.Brands, "BrandName", "BrandName");
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Sku,Quantity,BrandsName,StoresName,Availability")] Products products,IFormCollection form,string StoreName,string BrandName)
        {
            if (id != products.Id)
            {
                return NotFound();
            }
            var brand = _context.Brands.FirstOrDefault(x => x.BrandName == BrandName);
            if (brand is not null)
            {
                products.Brand = brand;
                // products.BrandsName = brand.Id;
            }

            var store = _context.Stores.FirstOrDefault(x => x.StoresName == StoreName);
            if (store is not null)
            {
                products.Store = store;
                // products.StoresName = store.Id;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                    var attList = new List<string>();
                    var attributes = _context.Attributes.ToArray();

                    foreach (var attr in attributes)
                    {
                        var existingProductAttribute = _context.ProductAttributes.FirstOrDefault(pa => pa.AttributeId == attr.Id && pa.ProductId == products.Id);
                        if (existingProductAttribute != null)
                        {
                            // Update existing record
                            existingProductAttribute.Value = form["dynamic" + attr.AttributeName].ToString();
                        }
                        else
                        {
                            // Create a new record (if needed)
                            _context.ProductAttributes.Add(new ProductAttribute
                            {
                                Attribute = attr,
                                Value = form["dynamic" + attr.AttributeName].ToString(),
                                Product = products
                            });
                        }
                    }

                    _context.SaveChanges();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
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
            ViewData["StoresName"] = new SelectList(_context.Stores, "Id", "Id", products.StoreId);
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Store)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            if (products != null)
            {
                _context.Products.Remove(products);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public JsonResult ActiveAttributes()

        {

          //  var attributes = _context.Attributes.Where(x=>x.Status==0);
            
            var attributes = from m in _context.Attributes where m.Status == Attributes.StatusType.Active select m;
            return Json(attributes);
        }
    }
}
