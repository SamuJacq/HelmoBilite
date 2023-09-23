using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetWeb.Models;
using ProjetWeb.ViewModels;

namespace ProjetWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatAdminController : Controller
    {
        private readonly _DbContext _context;
        


        public StatAdminController(_DbContext context) { 
            _context = context;
        }


        // GET: StatAdminController
        public async Task<IActionResult> Index(String filter = "Driver")
        {
            ViewData["Filter"] = filter;
            setFilter(filter);
            return View(await _context.Orders.ToListAsync());
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter([Bind("Name, NameDriver, NameCustomer, StartDate, EndDate")] StatFilterViewModel filter)
        {
            ViewData["Filter"] = filter.Name;
            setFilter(filter.Name);
            var o = Order.GetBy(filter, _context.Orders, _context.Drivers, _context.Customers);
            return View("Index", o);
        }


        // GET: StatAdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StatAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Produces("application/json")]
        public async Task<IActionResult> DriverName() {
            string term = HttpContext.Request.Query["term"].ToString();
            var list = await _context.Drivers.Where(u => u.Name.Contains(term)).Select(u => u.Name).ToListAsync();
            return Ok(list);
        }

        [Produces("application/json")]
        public async Task<IActionResult> CustomerName()
        {
            string term = HttpContext.Request.Query["term"].ToString();
            var list = await _context.Customers.Where(u => u.Name.Contains(term)).Select(u => u.Name).ToListAsync();
            return Ok(list);
        }

        private void setFilter(String filter) {
            if (filter.Equals("Driver"))
            {
                ViewBag.Driver = Admin.SetSelectDriver(_context.Drivers);
            }
            else if (filter.Equals("Customer"))
            {
                ViewBag.Customer = Admin.SetSelectCustomer(_context.Customers);
            }
        }
    }
}
