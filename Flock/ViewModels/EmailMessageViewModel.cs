using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class SampleOrderEmailMessageViewModel : BaseViewModel
    {
        public List<Mertex_Market> MertexMarkets { get; set; }

        public List<Language> Languages { get; set; }

        public List<TradeAccount> TradeAccounts { get; set; }

        public SampleOrderEmailMessageViewModel(UnitOfWork<FlockDataContext> UoW)
        {
            MertexMarkets = UoW.GetRepository<Mertex_Market>().GetAll().OrderBy(p => p.Code).ToList();

            Languages = UoW.GetRepository<Language>().GetAll().OrderBy(p => p.LanguageID).ToList();

            TradeAccounts = UoW.GetRepository<TradeAccount>().GetAll().OrderBy(p => p.Code).ToList();
        }
    }
}