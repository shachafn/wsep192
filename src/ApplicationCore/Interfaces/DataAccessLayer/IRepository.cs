using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Interfaces.DataAccessLayer
{
    public interface IRepository<T>
    {
        ICollection<T> FetchAll();
        T FindByIdOrNull(Guid id);
        IQueryable<T> Query();
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);

        /// <summary>
        /// Only For Tests.
        /// </summary>
        void Clear();
    }
}
