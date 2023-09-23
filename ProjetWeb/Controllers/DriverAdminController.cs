using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetWeb.Models;
using ProjetWeb.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using static NuGet.Packaging.PackagingConstants;

namespace ProjetWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DriverAdminController : Controller
    {
        private readonly _DbContext _context;
        public DriverAdminController(_DbContext context)
        {
            _context = context;
        }

        // GET: MemberAdminController
        public async Task<IActionResult> Index()
        {
            var listDriver = Driver.GetAllDrivers(_context.Drivers);
            return View(listDriver);
        }

        // GET: MemberAdminController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Drivers == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                return View(new ListDriverViewModel
                {
                    Id = driver.Id,
                    Name = driver.Name,
                    Matricule = driver.Matricule,
                    Email = driver.Email,
                    License = driver.License,
                });
            }
            return NotFound();
        }

        // GET: MemberAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberAdminController/Create
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

        // GET: MemberAdminController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Drivers == null)
            {
                return NotFound();
            }

            List<SelectListItem> ListLicense = new List<SelectListItem>();
            ListLicense.Add(new SelectListItem { Text = "C", Value = "C" });
            ListLicense.Add(new SelectListItem { Text = "CE", Value = "CE" });
            ViewBag.Licenses = ListLicense;

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(new ListDriverViewModel
            {
                Id = driver.Id,
                Name = driver.Name,
                Matricule = driver.Matricule,
                Email = driver.Email,
                License = driver.License,
            });
        }

        // POST: MemberAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IFormCollection collection)
        {
            try
            {
                var driver = await _context.Drivers.FindAsync(id);
                driver.License = collection["License"];

                _context.Update(driver);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: MemberAdminController/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Drivers == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                var driverViewModel = new ListDriverViewModel
                {
                    Id = driver.Id,
                    Name = driver.Name,
                    Matricule = driver.Matricule,
                    Email = driver.Email,
                    License = driver.License,
                };
                return View(driverViewModel);
            }
            return NotFound();
        }

        // POST: MemberAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, IFormCollection collection)
        {
            var driver = await _context.Drivers.FindAsync(id);

            if (driver != null) {
                Order.UpdateOrderRemoveDriver(id, _context.Orders);
                await _context.SaveChangesAsync();
                _context.Drivers.Remove(driver);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));   
        }
    }
}
