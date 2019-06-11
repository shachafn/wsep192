using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentaitionLayer.Models;

namespace PresentaitionLayer.Controllers
{
    public class HomeController : Controller
    {

        ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            var exceptionData = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionData != null)
            {
                ViewData["ExceptionMessage"] = exceptionData.Error.Message;
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
