using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.DAL
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : FlockDataContext, new()
    {
        private readonly FlockDataContext _Context;
        private Dictionary<Type, object> _repositories;
        private bool _disposed;

        // Default constructor that news the context and the dictionary containing all the repositories
        public UnitOfWork()
        {
            _Context = new TContext();
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }
        
        // Retrieves the DataContext for e.g. Stored Procedures
        public FlockDataContext Context
        {
            get { return _Context; }
        }

        // Retrieves the repository for some Model class T
        // If it doesn't exist, we create an instance of it
        public IFlockRepository<TY> GetRepository<TY>() where TY : class
        {
            // Checks if the Dictionary Key contains the Model class
            if (_repositories.Keys.Contains(typeof(TY)))
            {
                // Return the repository for that Model class
                return _repositories[typeof(TY)] as IFlockRepository<TY>;
            }

            // If the repository for that Model class doesn't exist, then create it
            var repository = new FlockRepository<TY>(_Context);

            // Add it to the dictionary
            _repositories.Add(typeof(TY), repository);

            // Return it
            return repository;
        }

        // Saves all changes done to the context
        public void Save()
        {
            _Context.SubmitChanges();
        }

        public bool Disposed
        {
            get { return _disposed; }
        }

        // Disposes the context
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Disposes the context only once if not disposed
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                    _Context.Dispose();

                this._disposed = true;
            }
        }
    }


    //public class UnitOfMetalWork<TContext> : IUnitOfWork where TContext : Flock.Models.Metal.FlockSQLMetalDataContext, new()
    //{
    //    private readonly Flock.Models.Metal.FlockSQLMetalDataContext _Context;
    //    private Dictionary<Type, object> _repositories;
    //    private bool _disposed;

    //    // Default constructor that news the context and the dictionary containing all the repositories
    //    public UnitOfMetalWork()
    //    {
    //        _Context = new TContext();
    //        _repositories = new Dictionary<Type, object>();
    //        _disposed = false;
    //    }

    //    // Retrieves the DataContext for e.g. Stored Procedures
    //    public Flock.Models.Metal.FlockSQLMetalDataContext Context
    //    {
    //        get { return _Context; }
    //    }

    //    // Retrieves the repository for some Model class T
    //    // If it doesn't exist, we create an instance of it
    //    public IFlockRepository<TY> GetRepository<TY>() where TY : class
    //    {
    //        // Checks if the Dictionary Key contains the Model class
    //        if (_repositories.Keys.Contains(typeof(TY)))
    //        {
    //            // Return the repository for that Model class
    //            return _repositories[typeof(TY)] as IFlockRepository<TY>;
    //        }

    //        // If the repository for that Model class doesn't exist, then create it
    //        var repository = new FlockSQLMetalRepository<TY>(_Context);

    //        // Add it to the dictionary
    //        _repositories.Add(typeof(TY), repository);

    //        // Return it
    //        return repository;
    //    }

    //    // Saves all changes done to the context
    //    public void Save()
    //    {
    //        _Context.SubmitChanges();
    //    }

    //    public bool Disposed
    //    {
    //        get { return _disposed; }
    //    }

    //    // Disposes the context
    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    // Disposes the context only once if not disposed
    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!this._disposed)
    //        {
    //            if (disposing)
    //                _Context.Dispose();

    //            this._disposed = true;
    //        }
    //    }
    //}
}