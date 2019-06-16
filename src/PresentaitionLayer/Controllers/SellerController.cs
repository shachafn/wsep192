using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentaitionLayer.Models;
using PresentaitionLayer.Models.SellerModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresentaitionLayer.Controllers
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
            try
            {
                var shops = _serviceFacade.GetUserShops(new Guid(HttpContext.Session.Id));
                return View(shops);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [HttpPost]
        public IActionResult OpenShop(string ShopName)
        {
            try
            {
                _serviceFacade.OpenShop(new Guid(HttpContext.Session.Id), ShopName);
                return RedirectToAction("MyShops", "Seller");
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        public IActionResult ReOpenShop(string shopId)
        {
            try
            {
                _serviceFacade.ReopenShop(new Guid(HttpContext.Session.Id), new Guid(shopId));
                return RedirectToAction("MyShops", "Seller");
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
        public IActionResult CloseShop(string shopId)
        {
            try
            {
                _serviceFacade.CloseShop(new Guid(HttpContext.Session.Id), new Guid(shopId));
                return RedirectToAction("MyShops", "Seller");
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
        // [HttpPost]
        public IActionResult Manage(string shopId)
        {
            try
            {
                ViewData["ShopId"] = shopId;
                ViewData["UserName"] = User.Identity.Name;
                var shops = _serviceFacade.GetUserShops(new Guid(HttpContext.Session.Id));
                var shop = shops.FirstOrDefault(currshop => currshop.Guid.Equals(new Guid(shopId)));
                ShopManageIndexModel model = new ShopManageIndexModel(shop)
                {
                    Owners = GetOwnerNames(shop),
                    Managers = GetManagerNames(shop)
                };
                if (shop.candidate != null)
                {
                    model.OwnerCandidate = (_serviceFacade.GetUserName(shop.candidate.OwnerGuid), _serviceFacade.GetUserName(shop.candidate.AppointerGuid));
                }
                model.CreatorName = _serviceFacade.GetUserName(shop.Creator.OwnerGuid);
                return View(model);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
        private List<(string, string,Guid)> GetOwnerNames(Shop shop)
        {
            List<(string, string, Guid)> owners = new List<(string, string,Guid)>();
            foreach (ShopOwner owner in shop.Owners)
            {
                owners.Add((_serviceFacade.GetUserName(owner.OwnerGuid),_serviceFacade.GetUserName(owner.AppointerGuid),owner.OwnerGuid) );
            }
            return owners;
        }
        private List<(string, string, string, Guid)> GetManagerNames(Shop shop)
        {
            List<(string, string, string, Guid)> managers = new List<(string, string, string, Guid)>();
            foreach (ShopOwner manager in shop.Managers)
            {
                managers.Add((_serviceFacade.GetUserName(manager.OwnerGuid), _serviceFacade.GetUserName(manager.AppointerGuid),
                    manager.Privileges.Count == 0 ? "None" : string.Join('\n', manager.Privileges.Select(p=>p.ToString())), manager.OwnerGuid));
            }
            return managers;
        }
        public IActionResult Products(string ShopId)
        {
            try
            {
                var products = _serviceFacade.GetShopProducts(new Guid(HttpContext.Session.Id), new Guid(ShopId));
                ViewData["ShopId"] = ShopId;
                return View(products);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
        [HttpPost]
        public IActionResult CreateItem(string ProductName,double Price,int StoredQuantity,string Category, string shopId)
        {
            try
            {
                _serviceFacade.AddProductToShop(new Guid(HttpContext.Session.Id), new Guid(shopId), ProductName, Category, Price, StoredQuantity);
                return RedirectToAction("Products", "Seller", new { ShopId = shopId });
            }
            catch(IllegalArgumentException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "Please fill in all required fields in a valid manner.");
                return View("UserMessage", message);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [HttpPost]
        public IActionResult ProductPolicies(string ShopId, string ProductId)
        {
            ViewData["ShopId"] = ShopId;
            ViewData["ProductID"] = ProductId;
            return View();
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
            try
            {
                ViewData["ShopId"] = ShopId;
                _serviceFacade.EditProductInShop(new Guid(HttpContext.Session.Id), new Guid(ShopId), product.Guid, product.Price, product.Quantity);
                return RedirectToAction("Products", "Seller", new { ShopId = ShopId });
            }
            catch (IllegalArgumentException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "Please fill in all required fields in a valid manner.");
                return View("UserMessage", message);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
        [HttpPost]
        public IActionResult DeleteItem(string ShopId, string ProductId)
        {
            try
            {
                _serviceFacade.RemoveProductFromShop(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(ProductId));
                return RedirectToAction("Products", "Seller", new { ShopId = ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        
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
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
        [HttpPost]
        public IActionResult AddCartDiscountPolicy(string Description, string Sign, int Than, int Percent, string ShopId)
        {
            try
            {
                _serviceFacade.AddNewDiscountPolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Cart discount policy", (object)Sign, (object)Than, (object)Percent, (object)Description, (object)null);
                return RedirectToAction("Policies", "Seller", new { ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [HttpPost]
        public IActionResult AddCompoundDiscountPolicy(string Description, int guid1,string Sign, int guid2, int Percent, string ShopId)
        {
            try
            {
                _serviceFacade.AddNewDiscountPolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Compound discount policy", (object)new Guid(guid1), (object)Sign, (object)new Guid(guid1), (object)Percent, (object)Description);
                return RedirectToAction("Policies", "Seller", new { ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
            PoliciesModel policy = new PoliciesModel();
            var shops = _serviceFacade.GetUserShops(new Guid(HttpContext.Session.Id));
            var shop = shops.FirstOrDefault(currshop => currshop.Guid.Equals(new Guid(ShopId)));
            policy.DiscountPolicies = shop.DiscountPolicies;
            _serviceFacade.AddNewDiscountPolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Compound discount policy",(object) policy.DiscountPolicies.ElementAt(guid1-1).Guid,(object)Sign, (object)policy.DiscountPolicies.ElementAt(guid2 - 1).Guid, (object)Percent, (object)Description);
            return RedirectToAction("Policies", "Seller", new {  ShopId });
        }

        [HttpPost]
        public IActionResult AddCompoundPurchasePolicy(string Description, int guid1, string Sign, int guid2, int Percent, string ShopId)
        {
            try
            {
                _serviceFacade.AddNewPurchasePolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Compound discount policy", (object)new Guid(guid1), (object)Sign, (object)new Guid(guid1), (object)Description);
                return RedirectToAction("Policies", "Seller", new { ShopId = ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
            PoliciesModel policy = new PoliciesModel();
            var shops = _serviceFacade.GetUserShops(new Guid(HttpContext.Session.Id));
            var shop = shops.FirstOrDefault(currshop => currshop.Guid.Equals(new Guid(ShopId)));
            policy.PurchasePolicies = shop.PurchasePolicies;
            _serviceFacade.AddNewPurchasePolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Compound discount policy", policy.PurchasePolicies.ElementAt(guid2 - 1).Guid, (object)Sign, policy.PurchasePolicies.ElementAt(guid2 - 1).Guid, (object)Description);
            return RedirectToAction("Policies", "Seller", new { ShopId = ShopId });
        }
        /*
                [HttpPost]
                public IActionResult AddPurchasePolicy(string ShopId)
                {
                    _serviceFacade.CascadeRemoveShopOwner(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(OwnerId));
                    return RedirectToAction("Manage", "Seller", new { ShopId = ShopId });
                }

                [HttpPost]
                public IActionResult AddDiscountPolicy(string ShopId)
                {
                    _serviceFacade.CascadeRemoveShopOwner(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(OwnerId));
                    return RedirectToAction("Manage", "Seller", new { ShopId = ShopId });
                }*/

        [HttpPost]
        public IActionResult AddProductPurchasePolicies(string Description,string Sign,int Than, string ProductId,string ShopId)
        {
            try
            {
                _serviceFacade.AddNewPurchasePolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Product purchase policy", (object)new Guid(ProductId), (object)Sign, (object)Than, (object)Description);
                return RedirectToAction("Products", "Seller", new { ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [HttpPost]
        public IActionResult AddProductDiscountPolicies(string Description, string Sign, int Than,int Percent, string ProductId, string ShopId)
        {
            try
            {
                _serviceFacade.AddNewDiscountPolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Product discount policy", (object)new Guid(ProductId), (object)Sign, (object)Than, (object)Percent, (object)Description);
                return RedirectToAction("Products", "Seller", new { ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [HttpPost]
        public IActionResult DeleteOwner(string OwnerId, string ShopId)
        {
            try
            {
                _serviceFacade.CascadeRemoveShopOwner(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(OwnerId));
                return RedirectToAction("Manage", "Seller", new { ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        [HttpPost]
        public IActionResult AddOwner(string OwnerName, string ShopId)
        {
            try
            {
                Guid OwnerId = _serviceFacade.GetUserGuid(OwnerName); //need to adress the empty guid thingy
                _serviceFacade.AddShopOwner(new Guid(HttpContext.Session.Id), new Guid(ShopId), OwnerId);
                return RedirectToAction("Manage", "Seller", new { ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        public IActionResult AddShopManager(string ManagerName, string ShopId, bool manageProducts = false,
            bool manageShopState = false, bool managePolicies = false, bool appointManagers = false)
        {
            try
            {
                Guid OwnerId = _serviceFacade.GetUserGuid(ManagerName); //need to adress the empty guid thingy
                ICollection<bool> privileges = new List<bool>
                {
                manageProducts, manageShopState, managePolicies, appointManagers
                };
                _serviceFacade.AddShopManager(new Guid(HttpContext.Session.Id), new Guid(ShopId), OwnerId, privileges.ToList());
                return RedirectToAction("Manage", "Seller", new { ShopId });
            }
            catch(BrokenConstraintException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "User is already a manager.");
                return View("UserMessage", message);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        public IActionResult DeleteManager(string ManagerId, string ShopId)
        {
            try
            {
                _serviceFacade.RemoveShopManager(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(ManagerId));
                return RedirectToAction("Manage", "Seller", new { ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        public IActionResult CancelOwnerAssignment(string ShopId)
        {
            try
            {
                _serviceFacade.cancelOwnerAssignment(new Guid(HttpContext.Session.Id), new Guid(ShopId));
                return RedirectToAction("Manage", "Seller", new { ShopId });
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Seller");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
    }
}