using ApplicationCore.Entitites;
using ApplicationCore.Mapping;
using DataAccessLayer.DAOs;
using DataAccessLayer.DAOs.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Mappers
{
    public class PrdoductDAOMapper: IGenericMapper<ProductDAO, Product>
    {
        BaseMapingManager _baseMapingManager;

        public PrdoductDAOMapper(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<ProductDAO, Product>(this);
        }
        Product IGenericMapper<ProductDAO, Product>.Map(ProductDAO fromObject)
        {
            Product mappedObject = new Product();
            mappedObject.thisGuid = fromObject.Guid;
            mappedObject.Name = fromObject.Name;
            mappedObject.Category = fromObject.Category;
            //mappedObject.Keywords = new List<string>(); //Do not support keywards as of now.

            return mappedObject;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
