using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApplicationCore.Interfaces.ServiceLayer;
using System.Linq;
using PressentaitionLayer.Models.AdminModels;
using System;

namespace PressentaitionLayer.Controllers
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
            return View(list.Select(b => new UsersModel()
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
    }
}