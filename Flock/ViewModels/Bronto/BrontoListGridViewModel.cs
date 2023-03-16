using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels.Bronto
{
    public class BrontoListGridViewModel
    {
        public Byte SubAccountID { get; set; }

        public List<ContactList> Results { get; set; }
        public BrontoListGridViewModel(Byte subAccountId, UnitOfWork<FlockDataContext> UoW)
        {
            SubAccountID = subAccountId;

            Results = UoW.GetRepository<ContactList>().GetAll().Where(p => p.SubAccountID == subAccountId).ToList();
        }
    }
}