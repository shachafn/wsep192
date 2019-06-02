using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Mapping
{
    public interface IMapper
    {
        object Map(object from);
    }
}
