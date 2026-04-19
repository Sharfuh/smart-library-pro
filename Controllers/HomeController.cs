using Microsoft.AspNetCore.Mvc;

namespace SmartLibraryPro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Welcome()
        {
            return View();
        }
    }
}