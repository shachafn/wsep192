using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Mapping
{
    public class BaseMapingManager
    {
        Dictionary<Tuple<Type, Type>, IMapper> _mappers = new Dictionary<Tuple<Type, Type>, IMapper>();

        public To Map<From, To>(From fromObject)
        {
            var matchingMapper = GetMatchingMapper<From, To>();
            if (matchingMapper == null)
                return default(To);
            return ((IGenericMapper<From,To>)matchingMapper).Map(fromObject);
        }

        private IMapper GetMatchingMapper<From, To>()
        {
            var types = new Tuple<Type, Type>(typeof(From), typeof(To));
            if (_mappers.ContainsKey(types))
                return _mappers[types];
            else
                return null;
        }

        public void AddMapper<From,To>(IMapper mapper)
        {
            _mappers.Add(new Tuple<Type, Type>(typeof(From), typeof(To)), mapper);
        }
    }
}
