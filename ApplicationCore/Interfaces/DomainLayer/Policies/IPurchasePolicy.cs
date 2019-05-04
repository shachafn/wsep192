﻿using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;

namespace DomainLayer.Policies
{
    public interface IPurchasePolicy
    {

        bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user);

    }
}