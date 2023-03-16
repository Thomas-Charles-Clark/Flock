using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels.Bronto
{
    public class BrontoSubAccountListsViewModel
    {
        public Byte SubAccountID { get; set; }

        public List<ContactList> Lists { get; set; }

        public BrontoSubAccountListsViewModel(Byte subAccountId, UnitOfWork<FlockDataContext> UoW)
        {
            SubAccountID = subAccountId;

            Lists = UoW.GetRepository<ContactList>().GetAll().Where(p => p.SubAccountID == subAccountId).ToList();
        }
    } 
}