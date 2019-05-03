﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PressentaitionLayer.Controllers
{
    [Authorize(Roles ="Buyer")]
    public class BuyerController : Controller
    {
        ILogger<BuyerController> _logger;

        public BuyerController(ILogger<BuyerController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Search()
        {
            return View();
        }
    }
}