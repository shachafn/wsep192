using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PressentaitionLayer.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        ILogger<SellerController> _logger;
        IServiceFacade _serviceFacade;
        public SellerController(IServiceFacade serviceFacade, ILogger<SellerController> logger)
        {
            _logger = logger;
            _serviceFacade = serviceFacade;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyShops()
        {
            _serviceFacade.
            return View();
        }
    }
}