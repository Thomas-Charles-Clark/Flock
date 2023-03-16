using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.DAL
{
    public interface IFlockRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAllDistinct();
        void Add(T entity);
        void Delete(T entity);
        void DeleteAll(IEnumerable<T> entity);
        void Update(T entity);
        bool Any();
    }
}