using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class MessageViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public MessageViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }
            
        public MessageViewModel Build(short? MessageID)
        {
            MessageViewModel Model = new MessageViewModel();
            Message m = UoW.GetRepository<Message>().GetAll().Where(p => p.MessageID == MessageID).Where(p => p.DeletedDT == null).Single();

            Model.MessageID = MessageID;
            Model.LanguageID = m.LanguageID;
            Model.PriceListID = m.PriceListID;
            Model.Threshold = m.Threshold;
            Model.BelowThresholdMessage = m.BelowThresholdMessage;
            Model.AboveThresholdMessage = m.AboveThresholdMessage;
            Model.NoThresholdMessage = m.NoThresholdMessage;
            Model.OnDate = m.OnDate;
            Model.OffDate = m.OffDate;
            Model.WebStatusID = UoW.Context.fnc_Status(Model.OnDate, Model.OffDate);
            if (Model.WebStatusID != null)
                Model.WebStatus = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == Model.WebStatusID).Select(p => p.Name).Single();

            Model.TradeSwitch = m.TradeSwitch;

            Model.CreatedDT = m.CreatedDT;
            Model.DeletedDT = m.DeletedDT;

            return Model;
        }
    }

    public class MessageViewModel : BaseViewModel
    {
        public short? MessageID { get; set; }

        public byte PriceListID { get; set; }

        public byte LanguageID { get; set; }

        public decimal? Threshold { get; set; }

        [StringLength(255)]
        public string BelowThresholdMessage { get; set; }

        [StringLength(255)]
        public string AboveThresholdMessage { get; set; }

        [Required]
        [StringLength(255)]
        public string NoThresholdMessage { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public byte? WebStatusID { get; set; }

        public string WebStatus { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime? DeletedDT { get; set; }

        public bool? TradeSwitch { get; set; }
    }
}