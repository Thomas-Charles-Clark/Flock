using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Web;

namespace Flock.ViewModels
{
    public class DeliveryExceptionsViewModel : BaseViewModel
    {
        public DeliveryExceptionsViewModel(UnitOfWork<FlockDataContext> UoW)
        {
            var globalVariable = UoW.GetRepository<GlobalVariable>().GetAll().Where(p => p.GlobalVariableID == 4).Single();

            DispatchDelay = globalVariable.Value;

            DelayList = globalVariable.ValueList.Elements().ToList().ConvertAll(c => new DelayListItem() { Text = c.Value, Value = c.Attribute("value").Value });

            DeliveryExceptions = UoW.GetRepository<DeliveryException>().GetAll().ToList();
        }

        public String DispatchDelay { get; set; }

        public List<DelayListItem> DelayList { get; set; }

        public List<DeliveryException> DeliveryExceptions { get; set; }
    }

    public class DelayListItem
    {
        public String Text { get; set; }

        public String Value { get; set; }
    }

}