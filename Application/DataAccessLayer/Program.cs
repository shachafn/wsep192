﻿using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using DataAccessLayer.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    class Program
    {
        public static void Main(string[] args)
        {
            var b = new BaseMapingManager();
            b.AddMapper<Shop, ShopDAO>(new ShopMapper());
            var x = b.Map<Shop, ShopDAO>(new Shop(Guid.NewGuid(), "hi"));
        }
    }
}
