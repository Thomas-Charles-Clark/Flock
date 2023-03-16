using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ViewModelBuilder
    {
        protected UnitOfWork<FlockDataContext> UoW;
    }

    public interface IViewModelBuilder<T>
    {
        T Build(long? id);
    }
}