using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Mapping
{
    public class ProductMapper : IGenericMapper<Product, ProductDAO>
    {
        ProductDAO IGenericMapper<Product, ProductDAO>.Map(Product fromObject)
        {
            ProductDAO mappedObject = new ProductDAO();
            mappedObject.thisGuid = fromObject.GetGuid();
            mappedObject.Name = fromObject.Name;
            mappedObject.Category = fromObject.Category;
            foreach (string s in fromObject.Keywords)
                mappedObject.Keywords.Add(); //stringWrapper
            return mappedObject;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
