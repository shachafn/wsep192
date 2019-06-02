using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApplicationCore.Interfaces.ServiceLayer;
using System.Linq;
using PresentaitionLayer.Models.AdminModels;
using System;
using System.Collections.Generic;
using ApplicationCore.Entitites;

namespace PresentaitionLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        IServiceFacade _facade;
        ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, IServiceFacade facade)
        {
            _logger = logger;
            _facade = facade;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var list = _facade.GetAllUsersExceptMe(new Guid(HttpContext.Session.Id));
            return View(list.Select(b => new AdminUsersDisplayModel()
            {
                Guid = b.Guid,
                Username = b.Username,
            }).ToList());
        }

        public IActionResult DeleteUser(Guid itemGuid)
        {
            _facade.RemoveUser(new Guid(HttpContext.Session.Id), itemGuid);
            return RedirectToAction("Users", "Admin");
        }

        public IActionResult Shops()
        {
            var shops = _facade.GetAllShops(new Guid(HttpContext.Session.Id));
            return View(shops.Select(s => new AdminShopDisplayModel()
            {
                Guid = s.Guid,
                CreatorGuid = s.Creator.OwnerGuid,
                State = s.ShopState
            }).ToList());
        }

        public IActionResult CloseShop(Guid shopGuid)
        {
            _facade.CloseShopPermanently(new Guid(HttpContext.Session.Id), shopGuid);
            return RedirectToAction("Shops", "Admin");
        }
    }
}