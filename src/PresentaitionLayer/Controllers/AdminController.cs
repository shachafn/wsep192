using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApplicationCore.Interfaces.ServiceLayer;
using System.Linq;
using PresentaitionLayer.Models.AdminModels;
using System;
using ApplicationCore.Exceptions;
using PresentaitionLayer.Models;

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
            try
            {
                var list = _facade.GetAllUsersExceptMe(new Guid(HttpContext.Session.Id));
                return View(list.Select(b => new AdminUsersDisplayModel()
                {
                    Guid = b.Guid,
                    Username = b.Username,
                }).ToList());
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Admin");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Admin");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        public IActionResult DeleteUser(Guid itemGuid)
        {
            try
            {
                _facade.RemoveUser(new Guid(HttpContext.Session.Id), itemGuid);
                return RedirectToAction("Users", "Admin");
            }
            catch(BrokenConstraintException)
            {
                var redirect = this.Url.Action("Index", "Admin");
                var message = new UserMessage(redirect, "The user is either the only owner of an active shop, or an admin himself. Could not complete the request.");
                return View("UserMessage", message);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Admin");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Admin");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        public IActionResult Shops()
        {
            try
            {
                var shops = _facade.GetAllShops(new Guid(HttpContext.Session.Id));
                return View(shops.Select(s => new AdminShopDisplayModel()
                {
                    Guid = s.Guid,
                    CreatorGuid = s.Creator.OwnerGuid,
                    ShopName = s.ShopName,
                    CreatorName = _facade.GetUserName(s.Creator.OwnerGuid),
                    State = s.ShopState
                }).ToList());
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Admin");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Admin");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        public IActionResult CloseShop(Guid shopGuid)
        {
            try
            {
                _facade.CloseShopPermanently(new Guid(HttpContext.Session.Id), shopGuid);
                return RedirectToAction("Shops", "Admin");
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Admin");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Admin");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }
    }
}