using InventoryWorld.Models;
using InventoryWorld.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace InventoryWorld.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Order.Include(o => o.OrderItems); // Include OrderItems to avoid lazy loading

            var chartData = orders.Select(o => new
            {
                OrderDate = o.OrderDate.ToString("MM/dd/yyyy"),
                TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.Rate) // Calculate total amount here
            });
            // var order = _context.Order.Select(o => new { OrderDate = o.OrderDate.ToString("MM/dd/yyyy"), TotalAmount = o.OrderItems.Sum(oi => oi.Amount) });
            var num = Convert.ToDecimal(_context.Order.Where(x => x.PaymentStatus == true).Count());
            var dem = Convert.ToDecimal( _context.Order.Count());
            var perc = num / dem *100;
            ViewBag.percentage = perc;
            return View(chartData);
        }

        public IActionResult Company()
        {
           var company = _context.Company.FirstOrDefault();
          

            return View(company);
        }


        [HttpPost]
        public IActionResult Company(Company company)
        {

            if (ModelState.IsValid) { 
           
            var existingCompany = _context.Company.First();
            if (existingCompany == null) { 
            
               _context.Company.Add(company);
                    _context.SaveChanges();
                    ViewBag.Alert = AlertServices.ShowAlert(AlertServices.Alerts.Success, "Company Details Successfully Added");
                    return View(company);

                }
            else
            {
              existingCompany.Address = company.Address;
             existingCompany.ServiceCharge = company.ServiceCharge;
                    existingCompany.VatCharge= company.VatCharge;
                    existingCompany.Phone= company.Phone;
                    existingCompany.Country= company.Country;
                    existingCompany.Currency= company.Currency;
                    existingCompany.Name= company.Name;
                _context.SaveChanges();
                    ViewBag.Alert = AlertServices.ShowAlert(AlertServices.Alerts.Success, "Company Details Successfully Updated");
                    return View(company);

                }

            }
            return View();


        }

        public IActionResult Reports()
        {

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
