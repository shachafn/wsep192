using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            var shops = _serviceFacade.GetUserShops(new Guid(HttpContext.Session.Id));
            return View(shops);
        }

        [HttpPost]
        public IActionResult OpenShop(string ShopName)
        {
            _serviceFacade.OpenShop(new Guid(HttpContext.Session.Id),ShopName);
            return RedirectToAction("MyShops", "Seller");
        }

        public IActionResult ReOpenShop(string shopId)
        {
            _serviceFacade.ReopenShop(new Guid(HttpContext.Session.Id), new Guid(shopId));
            return RedirectToAction("MyShops", "Seller");
        }
        public IActionResult CloseShop(string shopId)
        {
            _serviceFacade.CloseShop(new Guid(HttpContext.Session.Id), new Guid(shopId));
            return RedirectToAction("MyShops", "Seller");
        }
        // [HttpPost]
        public IActionResult Manage(string shopId)
        {
            ViewData["ShopId"] = shopId;
            ViewData["UserName"] = User.Identity.Name;
            var shops = _serviceFacade.GetUserShops(new Guid(HttpContext.Session.Id));
            var shop = shops.FirstOrDefault(currshop => currshop.Guid.Equals(new Guid(shopId)));
            ShopManageIndexModel model = new ShopManageIndexModel(shop);
            model.Owners = getOwnerNames(shop);
            if (shop.candidate != null)
            {
                model.ownerCandidate = (_serviceFacade.GetUserName(shop.candidate.OwnerGuid), _serviceFacade.GetUserName(shop.candidate.AppointerGuid));
            }
            model.creatorName = _serviceFacade.GetUserName(shop.Creator.OwnerGuid);
            return View(model);
        }
        private List<(string, string,Guid)> getOwnerNames(Shop shop)
        {
            List<(string, string,Guid)> owners = new List<(string, string,Guid)>();
            foreach (ShopOwner owner in shop.Owners)
            {
                owners.Add((_serviceFacade.GetUserName(owner.OwnerGuid),_serviceFacade.GetUserName(owner.AppointerGuid),owner.OwnerGuid) );
            }
            return owners;
        }
        private List<Tuple<string, string, int>> getManagerNames(Shop shop)
        {
            throw new NotImplementedException();
        }
        public IActionResult Products(string ShopId)
        {
            var products = _serviceFacade.GetShopProducts(new Guid(HttpContext.Session.Id), new Guid(ShopId));
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
        [HttpPost]
        public IActionResult AddCartDiscountPolicy(string Description, string Sign, int Than, int Percent, string ShopId)
        {
            _serviceFacade.AddNewDiscountPolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Cart discount policy", (object)Sign, (object)Than, (object)Percent, (object)Description,(object)null);
            return RedirectToAction("Policies", "Seller", new { ShopId = ShopId });
        }

        [HttpPost]
        public IActionResult AddCompoundDiscountPolicy(string Description, string guid1,string Sign, string guid2, int Percent, string ShopId)
        {
            _serviceFacade.AddNewDiscountPolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Compound discount policy",(object) new Guid(guid1),(object)Sign, (object)new Guid(guid1), (object)Percent, (object)Description);
            return RedirectToAction("Policies", "Seller", new { ShopId = ShopId });
        }

        [HttpPost]
        public IActionResult AddCompoundPurchasePolicy(string Description, string guid1, string Sign, string guid2, int Percent, string ShopId)
        {
            _serviceFacade.AddNewPurchasePolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Compound discount policy", (object)new Guid(guid1), (object)Sign, (object)new Guid(guid1), (object)Description);
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
            _serviceFacade.AddNewPurchasePolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Product purchase policy",(object)new Guid(ProductId), (object)Sign, (object)Than, (object)Description);
            return RedirectToAction("Products", "Seller", new { ShopId = ShopId });
        }

        [HttpPost]
        public IActionResult AddProductDiscountPolicies(string Description, string Sign, int Than,int Percent, string ProductId, string ShopId)
        {
            _serviceFacade.AddNewDiscountPolicy(new Guid(HttpContext.Session.Id), new Guid(ShopId), (object)"Product discount policy", (object)new Guid(ProductId), (object)Sign, (object)Than,(object)Percent, (object)Description);
            return RedirectToAction("Products", "Seller", new { ShopId = ShopId });
        }

        [HttpPost]
        public IActionResult DeleteRole(string OwnerId, string ShopId)
        {
            _serviceFacade.CascadeRemoveShopOwner(new Guid(HttpContext.Session.Id), new Guid(ShopId), new Guid(OwnerId));
            return RedirectToAction("Manage", "Seller", new { ShopId = ShopId });
        }

        [HttpPost]
        public IActionResult AddOwner(string OwnerName, string ShopId)
        {
            Guid OwnerId = _serviceFacade.GetUserGuid(OwnerName); //need to adress the empty guid thingy
            _serviceFacade.AddShopOwner(new Guid(HttpContext.Session.Id), new Guid(ShopId), OwnerId);
            return RedirectToAction("Manage", "Seller", new { ShopId = ShopId });
        }

        public IActionResult CancelOwnerAssignment(string ShopId)
        {
            _serviceFacade.cancelOwnerAssignment(new Guid(HttpContext.Session.Id), new Guid(ShopId));
            return RedirectToAction("Manage", "Seller", new { ShopId = ShopId });
        }
        /*public IActionResult Roles(string ShopId)
        {
            _serviceFacade.
            return View();
        }*/

    }
}