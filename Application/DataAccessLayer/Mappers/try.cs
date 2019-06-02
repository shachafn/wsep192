using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    class @try
    {
        public static void Main(string[] args)
        {
            var b = new BaseMapingManager();
            b.AddMapper<Shop, ShopDAO>(new ShopMapper());
        }
    }
}
