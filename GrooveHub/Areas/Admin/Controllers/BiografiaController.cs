using Microsoft.AspNetCore.Mvc;

namespace GrooveHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BiografiaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
