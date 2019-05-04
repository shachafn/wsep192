using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
            var shops = _serviceFacade.getUserShops(new Guid(HttpContext.Session.Id));
            return View(shops);
        }
        public IActionResult openShop()
        {
            _serviceFacade.OpenShop(new Guid(HttpContext.Session.Id));
            return RedirectToAction("MyShops", "Seller");
        }

       // [HttpPost]
        public IActionResult Manage(string shopId)
        {
            ViewData["ShopId"] = shopId;
            return View();
        }

        public IActionResult Products(string ShopId)
        {
            var products = _serviceFacade.getShopProducts(new Guid(HttpContext.Session.Id), new Guid(ShopId));
            ViewData["ShopId"] = ShopId;
            ViewData["pic"]=getRandomImage();
            return View(products);
        }
        [HttpPost]
        public IActionResult CreateItem(string ProductName,double Price,int StoredQuantity,string Category, string shopId)
        {
            _serviceFacade.AddProductToShop(new Guid(HttpContext.Session.Id), new Guid(shopId),ProductName,Category,Price, StoredQuantity);
            return RedirectToAction("Products","Seller",new { ShopId=shopId});
        }
        [HttpPost]
        public IActionResult EditItem()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public IActionResult DeleteItem()
        {
            throw new NotImplementedException();
        }

        private string getRandomImage()
        {
            System.Random rnd = new System.Random();
            var random = rnd.Next(1, 24);
            return $"~/images/Random/{random}.jpg";
        }
    }
}