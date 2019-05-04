using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PressentaitionLayer.Models.SellerModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var shops = _serviceFacade.getUserShops(new Guid(HttpContext.Session.Id));
            var shop = shops.FirstOrDefault(currshop => currshop.Guid.Equals(new Guid(shopId)));

            return View(new ShopManageIndexModel(shop));
        }

        public IActionResult Products(string ShopId)
        {
            var products = _serviceFacade.getShopProducts(new Guid(HttpContext.Session.Id), new Guid(ShopId));
            ViewData["ShopId"] = ShopId;
            return View(products);
        }
        [HttpPost]
        public IActionResult CreateItem(string ProductName,double Price,int StoredQuantity,string Category, string shopId)
        {
            _serviceFacade.AddProductToShop(new Guid(HttpContext.Session.Id), new Guid(shopId),ProductName,Category,Price, StoredQuantity);
            return RedirectToAction("Products","Seller",new { ShopId=shopId});
        }

        [HttpPost]
        public IActionResult EditItem(string ShopId,string ProductId)
        {
            ViewData["ShopId"] = ShopId;
            ViewData["ProductID"] = ProductId;
            return View();
        }
        [HttpPost]
        public IActionResult PostEditItem(ShopProduct product,string ShopId)
        {
            ViewData["ShopId"] = ShopId;
            _serviceFacade.EditProductInShop(new Guid(HttpContext.Session.Id), new Guid(ShopId), product.Guid,product.Price,product.Quantity);
            return RedirectToAction("Products","Seller", new { ShopId = ShopId});
        }
        [HttpPost]
        public IActionResult DeleteItem(string ShopId, string ProductId)
        {
            _serviceFacade.RemoveProductFromShop(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(ProductId));
            return RedirectToAction("Products", "Seller", new { ShopId = ShopId });
        }

        [HttpPost]
        public IActionResult DeleteRole(string OwnerId, string ShopId)
        {
            _serviceFacade.RemoveShopManager(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(OwnerId));
            return RedirectToAction("Manage", "Seller", new { ShopId = ShopId });
        }
        [HttpPost]
        public IActionResult AddOwner(string OwnerId, string ShopId)
        {
            _serviceFacade.AddShopOwner(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(OwnerId));
            return RedirectToAction("Manage", "Seller", new { ShopId = ShopId });
        }
        /*public IActionResult Roles(string ShopId)
        {
            _serviceFacade.
            return View();
        }*/

    }
}