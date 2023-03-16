using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class MertexMarketViewModel : BaseViewModel
    {
        public List<Country> AvailableCountries { get; set; }
        public List<Mertex_Market> MertexMarkets { get; set; }

        public MertexMarketViewModel()
        {

        }

        public MertexMarketViewModel(UnitOfWork<FlockDataContext> UoW)
        {
            this.AvailableCountries = UoW.GetRepository<Country_Language>().GetAll().Select(p => p.Country).ToList();

            this.MertexMarkets = UoW.GetRepository<Mertex_Market>().GetAll().ToList();
        }
    }
}