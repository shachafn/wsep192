using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentaitionLayer.Models.BuyerModels;
using System;
using System.Collections.Generic;

namespace PresentaitionLayer.Controllers
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

      /*  [AllowAnonymous]
        public IActionResult Details(Guid ItemId,Guid)
        {
            //model 
            return View();// need to pass a product according to id
        }*/
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult AddToCart( int Quantity,string ShopId,string ItemId)
        {
            _serviceFacade.AddProductToCart(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(ItemId), Quantity);
            if(User.IsInRole("Buyer"))
            {
                return RedirectToAction("Index", "Buyer");
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult ShoppingCart()
        {
            IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> bag = _serviceFacade.getUserBag(new Guid(HttpContext.Session.Id));
            CheckoutModel model = new CheckoutModel(bag);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult BuyNow(string ShopId)
        {
            _serviceFacade.PurchaseCart(new Guid(HttpContext.Session.Id),new Guid(ShopId));
            return RedirectToAction("ShoppingCart","Buyer");
        }
    }
}