using Flock.Classes;
using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class CallsToActionViewModel : BaseViewModel
    {
    }

    public class CallToActionEditModel
    {
        public int CallToActionID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(70)]
        public string Text { get; set; }

        [LinkageMutex("CallToActionArticleID", "CallToActionPageID", "CallToActionCollectionID", "CallToActionProductPageID", "CallToActionExternalURL", ErrorMessage = "Mutually Exclusive")]
        public int? CallToActionArticleID { get; set; }

        [LinkageMutex("CallToActionArticleID", "CallToActionPageID", "CallToActionCollectionID", "CallToActionProductPageID", "CallToActionExternalURL", ErrorMessage = "Mutually Exclusive")]
        public int? CallToActionPageID { get; set; }

        [LinkageMutex("CallToActionArticleID", "CallToActionPageID", "CallToActionCollectionID", "CallToActionProductPageID", "CallToActionExternalURL", ErrorMessage = "Mutually Exclusive")]
        public int? CallToActionProductPageID { get; set; }

        [LinkageMutex("CallToActionArticleID", "CallToActionPageID", "CallToActionCollectionID", "CallToActionProductPageID", "CallToActionExternalURL", ErrorMessage = "Mutually Exclusive")]
        public short? CallToActionCollectionID { get; set; }

        [StringLength(255)]
        [LinkageMutex("CallToActionArticleID", "CallToActionPageID", "CallToActionCollectionID", "CallToActionProductPageID", "CallToActionExternalURL", ErrorMessage = "Mutually Exclusive")]
        public string CallToActionExternalURL { get; set; }

        public bool NewWindow { get; set; }

        public short? CallToActionStyleID { get; set; }

        public byte? CallToActionWidthModeID { get; set; }

        public decimal? AppearanceDelay { get; set; }

        public decimal? VisibilityDuration { get; set; }

        public List<Style> AvailableStyles { get; set; }

        public List<CallToActionWidthMode> AvailableWidthModes { get; set; }

        public CallToActionEditModel()
        {

        }

        public CallToActionEditModel(UnitOfWork<FlockDataContext> UoW, int? callToActionID)
        {
            if (callToActionID == null) 
            {
                this.NewWindow = true;
                this.CallToActionStyleID = (short)1;
                this.CallToActionWidthModeID = (byte)1;
            }
            else
            {
                var modelItem = UoW.GetRepository<CallToAction>().GetAll().Where(p => p.CallToActionID == callToActionID).Single();

                this.CallToActionID = modelItem.CallToActionID;
                this.Name = modelItem.Name;
                this.Text = modelItem.Text;
                this.CallToActionArticleID = modelItem.ArticleID;
                this.CallToActionPageID = modelItem.PageID;
                this.CallToActionProductPageID = modelItem.ProductPageID;
                this.CallToActionCollectionID = modelItem.CollectionID;
                this.CallToActionExternalURL = modelItem.ExternalURL;
                this.NewWindow = modelItem.NewWindow;
                this.CallToActionStyleID = modelItem.StyleID;
                this.CallToActionWidthModeID = modelItem.CallToActionWidthModeID;
                this.AppearanceDelay = modelItem.AppearanceDelay;
                this.VisibilityDuration = modelItem.VisibilityDuration;
            }

            RebuildLists(UoW);
        }

        public void RebuildLists(UnitOfWork<FlockDataContext> UoW)
        {
            this.AvailableStyles = UoW.GetRepository<Style>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.StyleID).ToList();
            this.AvailableWidthModes = UoW.GetRepository<CallToActionWidthMode>().GetAll().ToList();
        }
    }
}