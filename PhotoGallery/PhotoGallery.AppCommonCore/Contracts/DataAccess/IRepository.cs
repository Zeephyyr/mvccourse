using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PhotoGallery.AppCommonCore.Contracts.DataAccess
{
    public interface IRepository<T> where T: class
    {
        void Add(T obj);

        void Update(T obj);

        T Get(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetEntities(Expression<Func<T, bool>> predicate);

        void Remove(T obj);
    }
}
