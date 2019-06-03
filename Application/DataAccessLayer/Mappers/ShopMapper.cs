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
            ShopDAO mappedObject = new ShopDAO();
            mappedObject.Guid = fromObject.GetGuid();
            foreach (ShopOwner owner in fromObject.Owners)
                mappedObject.Owners.Add(null);  //map owner in ShopOwnerMapper
            foreach (ShopOwner manager in fromObject.Managers)
                mappedObject.Owners.Add(null);  //map owner in ShopOwnerMapper
            foreach (ShopProduct shopProduct in fromObject.ShopProducts)
                mappedObject.ShopProducts.Add(null); //map shopProduct with shopProductmaapper
            switch (fromObject.ShopState)
            {
                case Shop.ShopStateEnum.Active:
                    mappedObject.ShopState = ShopDAO.ShopStateEnum.Active;
                    break;
                case Shop.ShopStateEnum.Closed:
                    mappedObject.ShopState = ShopDAO.ShopStateEnum.Closed;
                    break;
                case Shop.ShopStateEnum.PermanentlyClosed:
                    mappedObject.ShopState = ShopDAO.ShopStateEnum.PermanentlyClosed;
                    break;
            }
            mappedObject.ShopName = fromObject.ShopName;
            return mappedObject;
        }
    }
}
