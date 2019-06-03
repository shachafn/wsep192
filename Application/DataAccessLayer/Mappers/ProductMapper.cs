using ApplicationCore.Entitites;
using DataAccessLayer.DAOs;
using DataAccessLayer.DAOs.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Mapping
{
    public class ProductMapper : IGenericMapper<Product, ProductDAO>
    {
        BaseMapingManager _baseMapingManager;

        public ProductMapper(BaseMapingManager baseMapingManager)
        {
            _baseMapingManager = baseMapingManager;
            _baseMapingManager.AddMapper<Product, ProductDAO>(this);
        }
        ProductDAO IGenericMapper<Product, ProductDAO>.Map(Product fromObject)
        {
            ProductDAO mappedObject = new ProductDAO();
            mappedObject.Guid = fromObject.GetGuid();
            mappedObject.Name = fromObject.Name;
            mappedObject.Category = fromObject.Category;
            //mappedObject.Keywords = new List<StringWrapper>(); //Do not support keywards as of now.
            return mappedObject;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
