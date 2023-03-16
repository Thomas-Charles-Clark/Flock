using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IFlockRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        void Save();
    } 
}