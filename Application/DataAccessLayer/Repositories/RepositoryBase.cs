using DataAccessLayer.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        //private RepositoryContext _context;
        protected ApplicationContext Context { get; set; }

        public RepositoryBase(ApplicationContext context) => Context = context;

        public void Create(T entity)
        {
            this.Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            this.Context.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression).AsNoTracking();
        }

        public void Update(T entity)
        {
            this.Context.Set<T>().Update(entity);
        }
    }
}
