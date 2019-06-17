using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            try
            {
                if (ModelState.IsValid)
                {
                    var (isValid, user) = await _userServices.ValidateUserCredentialsAsync(model.UserName, model.Password, model.UserType.ToString(), new Guid(HttpContext.Session.Id));
                    if (isValid)
                    {
                        await LoginAsync(user);
                        return redirectAccordingToState(model.UserType.ToString(), model);
                    }
                    ModelState.AddModelError("InvalidCredentials", "invalid credentials");
                }

                return View(model);
            }
            catch(CredentialsMismatchException)
            {
                var redirect = this.Url.Action("Login", "Account");
                var message = new UserMessage(redirect, "Wrong credentials, please try again.");
                return View("UserMessage", message);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Login", "Account");
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

        private IActionResult redirectAccordingToState(string userType,LoginModel model)
        {
            try
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
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Home");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Home");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
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
            var isAdmin = _serviceFacade.IsUserAdmin(new Guid(HttpContext.Session.Id));
            if(isAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Country, "adminia"));
            }
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
            try
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
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Home");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Home");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
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
            try
            {
                if (ModelState.IsValid)
                {
                    var (isValid, user) = await _userServices.ValidateUserRegisterAsync(model.UserName, model.Password, new Guid(HttpContext.Session.Id));
                    if (isValid)
                    {
                        var redirect = this.Url.Action("Index", "Home");
                        var message = new UserMessage(redirect, "you have been registered successfully");
                        return View("UserMessage", message);
                        //return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("InvalidCredentials", "Registration failed, chosen username already exsists");
                }

                return View(model);
            }
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Login", "Account");
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

        [Route("UserMessage")]
        public async Task<IActionResult> UserMessage(UserMessage message)
        {
            return View(message);
        }

        [Route("NewState")]
        public async Task<IActionResult> NewStateAsync(string userType)
        {
            try
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
            catch (GeneralServerError)
            {
                var redirect = this.Url.Action("Index", "Home");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again.");
                return View("UserMessage", message);
            }
            catch (DatabaseConnectionTimeoutException)
            {
                var redirect = this.Url.Action("Index", "Home");
                var message = new UserMessage(redirect, "An error has occured. Please refresh and try again. (Database connection lost).");
                return View("UserMessage", message);
            }
        }

        private async Task changeRoleAsync(string userType)
        {
            try
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
                var isAdmin = _serviceFacade.IsUserAdmin(new Guid(HttpContext.Session.Id));
                if (isAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Country, "adminia"));
                }
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal, properties);
            }
            catch (GeneralServerError)
            {
                await Task.CompletedTask;
            }
            catch (DatabaseConnectionTimeoutException)
            {
                await Task.CompletedTask;
            }
        }
    }
}