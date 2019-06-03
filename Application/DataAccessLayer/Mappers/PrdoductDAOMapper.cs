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
        Product IGenericMapper<ProductDAO, Product>.Map(ProductDAO fromObject)
        {
            Product mappedObject = new Product();
            mappedObject.thisGuid = fromObject.Guid;
            mappedObject.Name = fromObject.Name;
            mappedObject.Category = fromObject.Category;
            foreach (StringWrapper s in fromObject.Keywords) // TODO: Convert it to stringwrapper.
                mappedObject.Keywords.Add(null); //stringWrapper
            return mappedObject;
        }

        object IMapper.Map(object from)
        {
            throw new NotImplementedException();
        }
    }
}
