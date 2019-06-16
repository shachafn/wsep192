using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentaitionLayer.Models;
using PresentaitionLayer.Models.BuyerModels;
using PresentaitionLayer.Models.SellerModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresentaitionLayer.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class BuyerController : Controller
    {
        ILogger<BuyerController> _logger;
        IServiceFacade _serviceFacade;
        public BuyerController(IServiceFacade serviceFacade, ILogger<BuyerController> logger)
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
            try
            {
                var results = _serviceFacade.SearchProduct(new Guid(HttpContext.Session.Id), strings, searchType);
                return View(results);
            }
            catch (IllegalArgumentException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "Please do a valid search operation.");
                return View("UserMessage", message);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [AllowAnonymous]
        public IActionResult Shops()
        {
            try
            {
                var shops = _serviceFacade.GetAllShops(new Guid(HttpContext.Session.Id));
                return View(shops);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [AllowAnonymous]
        public IActionResult ViewShop(string ShopId)
        {
            try
            {
                var products = _serviceFacade.GetShopProducts(new Guid(HttpContext.Session.Id), new Guid(ShopId));
                ViewData["ShopId"] = ShopId;
                return View(products);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [AllowAnonymous]
        public IActionResult Policies(string ShopId)
        {
            try
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
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
        /*  [AllowAnonymous]
          public IActionResult Details(Guid ItemId,Guid)
          {
              //model 
              return View();// need to pass a product according to id
          }*/

        [AllowAnonymous]
        [HttpPost]
        public IActionResult AddToCart(int Quantity, string ShopId, string ItemId)
        {
            try
            {
                _serviceFacade.AddProductToCart(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(ItemId), Quantity);
                if (User.IsInRole("Buyer"))
                {
                    return RedirectToAction("Index", "Buyer");
                }
                return RedirectToAction("Index", "Home");
            }
            catch (BrokenConstraintException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "Cant buy the same product.");
                return View("UserMessage", message);
            }
            catch (IllegalArgumentException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "Please buy more than 0 products.");
                return View("UserMessage", message);
            }
            catch (ShopStateException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "Cant add products to cart from a closed shop.");
                return View("UserMessage", message);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [AllowAnonymous]
        public IActionResult ShoppingCart()
        {
            try
            {
                IEnumerable<Tuple<ShoppingCart, IEnumerable<ShopProduct>>> bag = _serviceFacade.GetUserBag(new Guid(HttpContext.Session.Id));
                CheckoutModel model = new CheckoutModel(bag);
                IList<double> discountPrices = new List<double>();
                foreach (Tuple<ShoppingCart, IEnumerable<ShopProduct>> tup in bag)
                {
                    var disc = _serviceFacade.GetCartPrice(new Guid(HttpContext.Session.Id), tup.Item1.ShopGuid);
                    discountPrices.Add(disc);
                }
                model.AfterDiscount = discountPrices;
                return View(model);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult BuyNow(string ShopId)
        {
            try
            {
                _serviceFacade.PurchaseCart(new Guid(HttpContext.Session.Id), new Guid(ShopId));
                return RedirectToAction("ShoppingCart", "Buyer");
            }
            catch(ExternalServiceFaultException ex)
            {
                if (ex.Type.Equals(ExternalServiceFaultException.ExternalServiceType.Payment))
                {
                    var redirect = this.Url.Action("Index", "Buyer");
                    var message = new UserMessage(redirect, "Couldn't complete the payment, please try again later.");
                    return View("UserMessage", message);
                }
                else
                {
                    var redirect = this.Url.Action("Index", "Buyer");
                    var message = new UserMessage(redirect, "Couldn't complete the supply request, please try again later.");
                    return View("UserMessage", message);
                }
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult DeleteCartProduct(string shopGuid, string shopProductGuid)
        {
            try
            {
                _serviceFacade.RemoveProductFromCart(new Guid(HttpContext.Session.Id), new Guid(shopGuid), new Guid(shopProductGuid));
                return RedirectToAction("ShoppingCart", "Buyer");
            }
            catch (ShopStateException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "Cannot edit a cart of a closed shop.");
                return View("UserMessage", message);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult EditCartProduct(string shopGuid, string shopProductGuid, string newAmount)
        {
            try
            {
                _serviceFacade.EditProductInCart(new Guid(HttpContext.Session.Id), new Guid(shopGuid),
                    new Guid(shopProductGuid), int.Parse(newAmount));
                return RedirectToAction("ShoppingCart", "Buyer");
            }
            catch (ShopStateException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "Cannot edit a cart of a closed shop.");
                return View("UserMessage", message);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Buyer");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
    }
}