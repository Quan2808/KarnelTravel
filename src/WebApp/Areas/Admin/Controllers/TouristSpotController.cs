﻿using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class TouristSpotController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
