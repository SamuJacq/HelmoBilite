using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetWeb.Models;
using System.Globalization;
using System.Security.Claims;
using static NuGet.Packaging.PackagingConstants;

namespace ProjetWeb.Controllers
{
    public class DriverController : Controller
    {

        private readonly _DbContext _context;
        private readonly UserManager<User> _userManager;

        public DriverController(_DbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: DriverController
        public async Task<IActionResult> Index(string state = "all")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = Driver.GetOrders(_context.Orders, userId, state);

            var semaine = orders.GroupBy(o => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                o.StartDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));

            List<(DateTime, DateTime)> plagesSemaines = new List<(DateTime, DateTime)>();
            foreach (var semaineLivraisons in semaine)
            {
                var debutSemaine = semaineLivraisons.Min(livraison => livraison.StartDate);
                var finSemaine = semaineLivraisons.Max(livraison => livraison.StartDate);
                plagesSemaines.Add((debutSemaine, finSemaine));
            }

            List<DateTime> semainesCalendrier = plagesSemaines.Select(plage => plage.Item1).ToList();

            ViewBag.PlagesSemaines = plagesSemaines;
            ViewBag.SemainesCalendrier = semainesCalendrier;

            return View(orders);
        }


        // GET: DriverController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: DriverController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int Id, string State, string CommentFailed, string CommentSucess)
        {
            
            var order = _context?.Orders?.Where(o => o.Id == Id)?.First();

            switch (State.ToLower())
            {
                case "delivered":
                    order.Delivered = true;
                    order.Comment = CommentSucess;
                    break;
                default:
                    order.Delivered = false;
                    order.Comment = CommentFailed;
                    break;
            }
            
            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EndOrder(int id)
        {
            var order = _context?.Orders?.Where(o => o.Id == id).First();
            return View("Edit", order);
        }
    }
}
