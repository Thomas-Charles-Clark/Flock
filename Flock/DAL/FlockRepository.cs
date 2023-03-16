using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Flock.Models;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Data.Common;
using System.Web.Caching;
using NLog;

namespace Flock.DAL
{
    public class FlockRepository<T> : IFlockRepository<T> where T : class//, IDisposable
    {
        private FlockDataContext Context;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public FlockRepository(FlockDataContext ctx)
        {
            Context = ctx;
        }

        public virtual IQueryable<T> GetAll()
        {
            return Context.GetTable<T>();
        }

        public virtual IQueryable<T> GetAllDistinct()
        {
            return Context.GetTable<T>().Distinct();
        }

        public void Add(T entity)
        {
            Context.GetTable<T>().InsertOnSubmit(entity);
        }

        public void Delete(T entity)
        {
            Context.GetTable<T>().DeleteOnSubmit(entity);
        }

        public void DeleteAll(IEnumerable<T> entity)
        {
            Context.GetTable<T>().DeleteAllOnSubmit(entity);
        }

        public void Update(T entity)
        {
            var entry = Context.GetTable<T>().Where(s => s == entity);
            Context.GetTable<T>().Attach(entity);
        }

        public bool Any()
        {
            return Context.GetTable<T>().Any();
        }

        public virtual ITable GetTable()
        {
            return Context.GetTable<T>();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }    
    }


    public class FlockSQLMetalRepository<T> : IFlockRepository<T> where T : class//, IDisposable
    {
        private Flock.Models.Metal.FlockSQLMetalDataContext Context;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public FlockSQLMetalRepository(Flock.Models.Metal.FlockSQLMetalDataContext ctx)
        {
            Context = ctx;
        }

        public virtual IQueryable<T> GetAll()
        {
            return Context.GetTable<T>();
        }

        public virtual IQueryable<T> GetAllDistinct()
        {
            return Context.GetTable<T>().Distinct();
        }

        public void Add(T entity)
        {
            Context.GetTable<T>().InsertOnSubmit(entity);
        }

        public void Delete(T entity)
        {
            Context.GetTable<T>().DeleteOnSubmit(entity);
        }

        public void DeleteAll(IEnumerable<T> entity)
        {
            Context.GetTable<T>().DeleteAllOnSubmit(entity);
        }

        public void Update(T entity)
        {
            var entry = Context.GetTable<T>().Where(s => s == entity);
            Context.GetTable<T>().Attach(entity);
        }

        public bool Any()
        {
            return Context.GetTable<T>().Any();
        }

        public virtual ITable GetTable()
        {
            return Context.GetTable<T>();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}