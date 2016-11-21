using System;
using PhotoGallery.AppCommonCore.Contracts.DataAccess;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Linq;

namespace PhotoGallery.DataAccess
{
    public class EFRepository<T> : IRepository<T> where T: class
    {
        private readonly DbContext _dbContext;

        public EFRepository(DbContext context)
        {
            _dbContext = (DbContext)context;
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual IEnumerable<T> GetEntities(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().FirstOrDefault(predicate);
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
    {
            _dbContext.Entry<T>(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
