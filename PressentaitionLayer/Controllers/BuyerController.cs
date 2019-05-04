using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PressentaitionLayer.Models.BuyerModels;
using System;
using System.Collections.Generic;

namespace PressentaitionLayer.Controllers
{
    [Authorize(Roles ="Buyer")]
    public class BuyerController : Controller
    {
        ILogger<BuyerController> _logger;
        IServiceFacade _serviceFacade;
        public BuyerController(IServiceFacade serviceFacade,ILogger<BuyerController> logger)
        {
            _logger = logger;
            _serviceFacade = serviceFacade;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Search(string searchstring)
        {
            ViewData["searched"] = searchstring;
            List<string> strings = new List<string>();
            strings.Add(searchstring);
            var results =_serviceFacade.SearchProduct(new Guid(HttpContext.Session.Id), strings, "Name");
            return View(results);
        }

        [AllowAnonymous]
        public IActionResult Details(Guid ItemId)
        {
            //model 
            return View();// need to pass a product according to id
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult AddToCart( ShopProductBuyModel model)
        {
           // _serviceFacade.AddProductToCart(new Guid(HttpContext.Session.Id),model.Id,);
            return RedirectToAction("redirectAccordingToState", "Account");
            //model 
        }
    }
}