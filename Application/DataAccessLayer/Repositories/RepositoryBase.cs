﻿using ApplicationCore.IRepositories;
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
        protected ApplicationContext Context { get; set; }

        public RepositoryBase(ApplicationContext context) => Context = context;

        public virtual void Create(T entity)
        {
            this.Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            this.Context.Set<T>().Remove(entity);
        }

        public virtual  IQueryable<T> FindAll()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression).AsNoTracking();
        }

        public virtual void Update(T entity)
        {
            this.Context.Set<T>().Update(entity);
        }

        public virtual void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
