using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentaitionLayer.Models;
using PresentaitionLayer.Services;

namespace PresentaitionLayer.Account
{
    [Route("Account")]
    public class AccountController : Controller
    {
        IServiceFacade _serviceFacade;
        UserServices _userServices;
        ILogger<AccountController> _logger;
        public AccountController(UserServices userServices, IServiceFacade serviceFacade, ILogger<AccountController> logger)
        {
            _userServices = userServices;
            _serviceFacade = serviceFacade;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel());
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var (isValid, user) = await _userServices.ValidateUserCredentialsAsync(model.UserName, model.Password,model.UserType.ToString(), new Guid (HttpContext.Session.Id));
                if (isValid)
                {
                    await LoginAsync(user);
                    return redirectAccordingToState(model.UserType.ToString(),model);
                }
                ModelState.AddModelError("InvalidCredentials", "Invalid credentials.");
            }

            return View(model);
        }

        private IActionResult redirectAccordingToState(string userType,LoginModel model)
        {
            switch (userType)
            {
                case "Buyer":
                    return RedirectToAction("Index", "Buyer");
                case "Seller":
                    if (!_serviceFacade.ChangeUserState(new Guid(HttpContext.Session.Id), "SellerUserState"))
                    {
                        ModelState.AddModelError("Incorrect User type", "Incorrect User type");
                        _serviceFacade.Logout(new Guid(HttpContext.Session.Id));
                        return View(model);
                    }
                    return RedirectToAction("Index", "Seller");
                case "Admin":
                    if (!_serviceFacade.ChangeUserState(new Guid(HttpContext.Session.Id), "AdminUserState"))
                    {
                        ModelState.AddModelError("Incorrect User type", "Incorrect User type");
                        _serviceFacade.Logout(new Guid(HttpContext.Session.Id));
                        return View(model);
                    }
                    return RedirectToAction("Index", "Admin");
                 default:
                    ModelState.AddModelError("an error occured", "an error occured");
                    return View(model);
            }
        }

        private async Task LoginAsync(UserModel user)
        {
            var properties = new AuthenticationProperties
            {
                //AllowRefresh = false,
                IsPersistent = true,
                //ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(10)
            };
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role,user.UserType)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal, properties);
        }

        [Route("Logout")]
        [Authorize]
        public IActionResult Logout(string returnUrl)
        {  
            return View();
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                try
                {
                    _serviceFacade.Logout(new Guid(HttpContext.Session.Id));
                }
                catch (Exception)
                {

                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("Register")]
        public IActionResult Register(string returnUrl)
        {
            return View();
        }

        [Route("Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var (isValid, user) = await _userServices.ValidateUserRegisterAsync(model.UserName, model.Password,new Guid(HttpContext.Session.Id));
                if (isValid)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("InvalidCredentials", "Invalid credentials.");
            }

            return View(model);
        }

        [Route("NewState")]
        public async Task<IActionResult> NewStateAsync(string userType)
        {
            
            switch (userType)
            {
                case "Buyer":
                    if (!User.IsInRole("Buyer") && _serviceFacade.ChangeUserState(new Guid(HttpContext.Session.Id), "BuyerUserState"))
                    {
                        await changeRoleAsync(userType);
                        return RedirectToAction("Index", "Buyer");
                    }
                    return User.IsInRole("Seller") ? RedirectToAction("Index", "Seller") : RedirectToAction("Index", "Admin");
                case "Seller":
                    if (!User.IsInRole("Seller") && _serviceFacade.ChangeUserState(new Guid(HttpContext.Session.Id), "SellerUserState"))
                    {
                        await changeRoleAsync(userType);
                        return RedirectToAction("Index", "Seller");
                    }
                    return User.IsInRole("Buyer") ? RedirectToAction("Index", "Buyer") : RedirectToAction("Index", "Admin");
                case "Admin":
                    if (!User.IsInRole("Admin") && _serviceFacade.ChangeUserState(new Guid(HttpContext.Session.Id), "AdminUserState"))
                    {
                        await changeRoleAsync(userType);
                        return RedirectToAction("Index", "Admin");
                    }
                    return User.IsInRole("Seller") ? RedirectToAction("Index", "Seller") : RedirectToAction("Index", "Buyer");
                default:
                    return RedirectToAction("Index", "Home");
            }

        }

        private async Task changeRoleAsync(string userType)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties
            {
                //AllowRefresh = false,
                IsPersistent = true,
                //ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(10)
            };
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, User.Claims.First(c=>c.Type==ClaimTypes.NameIdentifier).Value),
                new Claim(ClaimTypes.Name,User.Claims.First(c=>c.Type==ClaimTypes.Name).Value),
                new Claim(ClaimTypes.Role,userType)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal, properties);
        }
    }
}