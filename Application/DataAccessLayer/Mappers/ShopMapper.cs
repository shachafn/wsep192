using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShopMapper : IGenericMapper<Shop, ShopDAO>
    {
        public object Map(object from)
        {
            throw new NotImplementedException();
        }

        ShopDAO IGenericMapper<Shop, ShopDAO>.Map(Shop fromObject)
        {
            return new ShopDAO();
        }
    }
}
