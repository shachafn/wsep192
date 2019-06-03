using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class ShoppingBagDAOCart : IGenericMapper<ShoppingBagDAO, ShoppingBag>
    {
        BaseMapingManager _baseMapingManager;

        public ShoppingBagDAOCart(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<ShoppingBagDAO, ShoppingBag>(this);
        }
        ShoppingBag IGenericMapper<ShoppingBagDAO, ShoppingBag>.Map(ShoppingBagDAO fromObject)
        {
            ShoppingBag bag = new ShoppingBag();
            bag.UserGuid = fromObject.UserGuid;
            bag.Guid = fromObject.BagGuid;
            foreach (ShoppingCartDAO cartDAO in fromObject.ShoppingCarts)
                bag.ShoppingCarts.Add(null); //Mapper of cartDAO to cart
            return bag;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
