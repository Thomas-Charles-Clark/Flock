using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels.Bronto
{
    public class BrontoListsViewModel : BaseViewModel
    {
        public List<SubAccount> SubAccounts { get; set; }

        public Int16? SelectedSubAccount { get; set; }

        public BrontoListsViewModel(UnitOfWork<FlockDataContext> UoW)
        {
            SubAccounts = UoW.GetRepository<SubAccount>().GetAll().ToList();
        }
    }
}