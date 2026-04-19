using Microsoft.AspNetCore.Mvc;

namespace SmartLibraryPro.Controllers
{
    public class AuthController : Controller
    {
        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "1234")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            return RedirectToAction("Welcome", "Home");
        }
    }
}