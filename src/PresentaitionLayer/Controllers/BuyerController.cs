using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentaitionLayer.Models.BuyerModels;
using PresentaitionLayer.Models.SellerModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult Search(string searchstring, string searchType)
        {
            ViewData["searched"] = searchstring;
            List<string> strings = new List<string>
            {
                searchstring == null ? "" : searchstring
            };
            var results =_serviceFacade.SearchProduct(new Guid(HttpContext.Session.Id), strings, searchType);
            return View(results);
        }

        [AllowAnonymous]
        public IActionResult Shops()
        {
            var shops = _serviceFacade.GetAllShops(new Guid(HttpContext.Session.Id));
            return View(shops);
        }

        [AllowAnonymous]
        public IActionResult ViewShop(string ShopId)
        {
            var products = _serviceFacade.GetShopProducts(new Guid(HttpContext.Session.Id), new Guid(ShopId));
            ViewData["ShopId"] = ShopId;
            return View(products);
        }

        [AllowAnonymous]
        public IActionResult Policies(string ShopId)
        {
            ViewData["ShopId"] = ShopId;
            var shops = _serviceFacade.GetUserShops(new Guid(HttpContext.Session.Id));
            var shop = shops.FirstOrDefault(currshop => currshop.Guid.Equals(new Guid(ShopId)));
            PoliciesModel model = new PoliciesModel();
            model.PurchasePolicies = shop.PurchasePolicies;
            model.DiscountPolicies = shop.DiscountPolicies;
            model.name = shop.ShopName;
            return View(model);
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
            IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> bag = _serviceFacade.GetUserBag(new Guid(HttpContext.Session.Id));
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

        [AllowAnonymous]
        [HttpPost]
        public IActionResult DeleteCartProduct(string shopGuid, string shopProductGuid)
        {
            _serviceFacade.RemoveProductFromCart(new Guid(HttpContext.Session.Id), new Guid(shopGuid), new Guid(shopProductGuid));
            return RedirectToAction("ShoppingCart", "Buyer");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult EditCartProduct(string shopGuid, string shopProductGuid, string newAmount)
        {
            _serviceFacade.EditProductInCart(new Guid(HttpContext.Session.Id), new Guid(shopGuid),
                new Guid(shopProductGuid), int.Parse(newAmount));
            return RedirectToAction("ShoppingCart", "Buyer");
        }
    }
}