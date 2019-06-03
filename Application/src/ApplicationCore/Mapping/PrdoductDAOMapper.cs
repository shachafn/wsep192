using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Mapping
{
    public class PrdoductDAOMapper: IGenericMapper<ProductDAO, Product>
    {
        Product IGenericMapper<ProductDAO, Product>.Map(ProductDAO fromObject)
        {
            Product mappedObject = new Product();
            mappedObject.thisGuid = fromObject.Guid();
            mappedObject.Name = fromObject.Name;
            mappedObject.Category = fromObject.Category;
            foreach (string s in fromObject.Keywords) // TODO: Convert it to stringwrapper.
                mappedObject.Keywords.Add(); //stringWrapper
            return mappedObject;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
