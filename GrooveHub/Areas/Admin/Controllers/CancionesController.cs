using Microsoft.AspNetCore.Mvc;

namespace GrooveHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CancionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agregar()
        {
            return View();
        }
    }
}
