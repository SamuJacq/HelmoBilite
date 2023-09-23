using Microsoft.AspNetCore.Mvc;

namespace ProjetWeb.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View();
        }
    }
}