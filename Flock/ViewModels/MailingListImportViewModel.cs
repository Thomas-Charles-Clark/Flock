using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class MailingListImportViewModel : BaseViewModel
    {
        [Required]
        public string ThirdParty { get; set; }
    }

    public class MailingListImportResultViewModel
    {
        public string FileName { get; set; }
        public string[] ErrorTexts { get; set; }
        public List<Flock.Controllers.MailingListExcelRow> UploadedRows { get; set; }
    }

    public class MailingListMergeResultViewModel
    {
        public List<Flock.Controllers.MailingListExcelRow> AllRecords { get; set; } //Total rows in spreadsheet (inc. invalid emails)
        public List<Flock.Controllers.MailingListExcelRow> InsertedRecords { get; set; }
        public List<Flock.Controllers.MailingListExcelRow> Returned { get; set; }
    }
}