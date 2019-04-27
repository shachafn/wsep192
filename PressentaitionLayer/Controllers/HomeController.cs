using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PressentaitionLayer.Models;
using PressentaitionLayer.Services;

namespace PressentaitionLayer.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }
        public IActionResult Index()
        {
            var paths = HttpContext.Session.GetString("actions") ?? string.Empty;
            HttpContext.Session.SetString("actions", paths + ";" + "Index");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Authorize(Roles ="Buyer")]
        public IActionResult Debug()
        {
            ViewData["SessionId"] = HttpContext.Session.Id;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
