using InventoryWorld.Models;
using InventoryWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace InventoryWorld.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
                _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Order.ToList();
            return View(model);
        }


        public IActionResult Details(int id)
        {
          //  var orderItems= _context.
            var order = _context.Order.Include(x=>x.OrderItems).FirstOrDefault(x=>x.OrderId  == id);
            if(order == null)
            {
                NotFound();
            }
            return View(order);
        }
        public IActionResult Create()
        {
            ViewData["Products"] = new SelectList(_context.Products.Where(x=>x.Quantity>0), "Price", "Name", "Please Select Store");
            ViewBag.Vat= _context.Company.FirstOrDefault().VatCharge;
            ViewBag.Service = _context.Company.FirstOrDefault().ServiceCharge;

            return View();
        }

        [HttpPost]
      [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody]JsonElement model) {
            Order order = new Order();
       //    order.OrderItems = new List<OrderItem>();
            List<OrderItem> orderItems = new List<OrderItem>();
           var customer= model[0].ToString();
            order.CustomerName = customer;

            var Phone = model[1].ToString();
            order.PhoneNumber = Phone;
            Random rnd =new Random();
            int  ordernnumber =rnd.Next(10000000, 99999999);
            order.OrderNo = $"INV{ordernnumber:D8}";
            order.OrderDate= DateTime.Now;


            var OrderItems = model[2].EnumerateArray;

            foreach (var item in OrderItems.Invoke())
            {
                var quantity = Convert.ToInt32(item.GetProperty("Quantity").ToString());
                var product = _context.Products.FirstOrDefault(x=>x.Name== item.GetProperty("ProductName").ToString());
                if(product != null)
                {
                    product.Quantity = product.Quantity - quantity;
                    _context.SaveChanges();
                }
                if (quantity > product.Quantity)
                {
                  ///  ModelState.AddModelError("Error", $"You can't order more than the available quantity for {product.Name}");
                    return Json( $"You can't order more than the available{product.Quantity} quantity for {product.Name}");
                }
                order.OrderItems.Add(new OrderItem {Quantity= quantity,Product=product,Rate=Convert.ToDecimal(product.Price) });
              
            }
            _context.Order.Add(order);
            _context.SaveChanges();

          //  TempData["Alert"] = AlertServices.ShowAlert(AlertServices.Alerts.Success, "Order Successfully Saved");
            Console.WriteLine(TempData["Alert"]);
            return RedirectToAction("Index");
         
        }

        public IActionResult PayOrder(int id ) {

            var ordertopay = _context.Order.FirstOrDefault(x => x.OrderId == id);

            if (ordertopay != null)
            {
                ordertopay.PaymentStatus = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        
        }

        
    }
}
