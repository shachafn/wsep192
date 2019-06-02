using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Mapping
{
    public interface IGenericMapper<GenericFrom, GenericTo> : IMapper
    {
        GenericTo Map(GenericFrom fromObject);
    }
}
