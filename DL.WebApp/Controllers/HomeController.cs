﻿using Microsoft.AspNetCore.Mvc;

namespace DL.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
