using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Model;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Introduction()
        {
            return View();
        }
    }
}
