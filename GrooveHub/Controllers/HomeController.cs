using Microsoft.AspNetCore.Mvc;

namespace GrooveHub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Biografia()
        {
            return View();
        }

        public IActionResult Cancion()
        {
            return View();
        }

        public IActionResult Discografia()
        {
            return View();
        }

        public IActionResult AparicionesEnTV()
        {
            return View();
        }
    }
}
