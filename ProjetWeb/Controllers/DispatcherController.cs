using Bogus.DataSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetWeb.Models;
using ProjetWeb.ViewModels;
using System.Data;
using System.Linq;
using System.Net;

namespace ProjetWeb.Controllers
{
    [Authorize(Roles = "Dispatcher")]
    public class DispatcherController : Controller
    {
        private readonly _DbContext _context;

        public DispatcherController(_DbContext context)
        {
            _context = context;
        }
        // GET: DispatcherController
        public ActionResult Index()
        {
            var orderListViewModel = Order.GetAllOrder(_context.Orders, _context.Customers);
            return View(orderListViewModel);
        }

        // GET: DispatcherController/Details/5
        public ActionResult Details(int id)
        {
            var detailsOrderDispatcherViewModel = Order.GetOrderDetail(id, _context.Orders, _context.Customers);

            return View(detailsOrderDispatcherViewModel);
        }

        // GET: DispatcherController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DispatcherController/Create
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

        // GET: DispatcherController/Edit/5
        public ActionResult Edit(int id)
        {
            TempData["message"] = "";

            var orderInfo = Order.GetDateOrder(id, _context.Orders);

            ViewBag.Driver = Driver.SetSelectDriver(orderInfo.FirstOrDefault().StartDate, orderInfo.FirstOrDefault().EndDate, _context.Orders, _context.Drivers);

            ViewBag.Truck = Truck.SetSelectTruck(orderInfo.FirstOrDefault().StartDate, orderInfo.FirstOrDefault().EndDate.AddHours(1), _context.Orders, _context.Trucks);

            return View();
        }

        // POST: DispatcherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            var LicenseTruck = await _context.Trucks.FindAsync(int.Parse(collection["IdTruck"]));
            var LicenseDriver = await _context.Drivers.FindAsync(collection["IdDriver"]);
            var order = Order.GetOrderById(id, _context.Orders);

            if (LicenseTruck.Types == "CE" && LicenseDriver.License == "C"){
                var error = "Ce conducteur n'a pas le permis pour conduire ce camion";
                ViewBag.Message = error;
                return Edit(id);
            }

            Order.UpdateOrderAccept(order, LicenseTruck, LicenseDriver, _context);

            var message = string.Format("Le conducteur {0} a été assigne à la livraison", LicenseDriver.Name);
            TempData["message"] = message;

            return RedirectToAction("Index");

        }

        // GET: DispatcherController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DispatcherController/Delete/5
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
    }
}
