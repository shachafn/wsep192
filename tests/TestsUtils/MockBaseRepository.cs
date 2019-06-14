using System;
using System.Collections.Generic;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace TestsUtils
{
    public class MockBaseRepository<T> : IRepository<T>
    {
        public MockContext MockContext;

        public MockBaseRepository(MockContext mockContext)
        {
            MockContext = mockContext;
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> FetchAll()
        {
            throw new NotImplementedException();
        }

        public T FindByIdOrNull(Guid id)
        {
            throw new NotImplementedException();
        }

        public System.Linq.IQueryable<T> Query()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
