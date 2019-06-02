using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ApplicationCore.Entities;

namespace ApplicationCore.IRepositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        T FindById(Guid id);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteAll();
        void DeleteById(params Guid[] ids);
    }
}
