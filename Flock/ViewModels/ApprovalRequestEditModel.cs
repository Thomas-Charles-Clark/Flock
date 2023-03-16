using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class approvalRequestShunt
    {
        public int ApprovalRequestID { get; set; }
        public string Id { get; set; }
        
        public User User { get; set; }

        public string ApprovalStatus { get; set; }

        public DateTime? ApprovalExpiryDT { get; set; }

        public DateTime? RequestDT { get; set; }

        public bool StockFeedAccess { get; set; }

        public bool BatchPieceStockAccess { get; set; }

        public bool UpcomingSeasonEarlyAccess { get; set; }
    }


    public class ApprovalRequestEditModel
    {
        public int ApprovalRequestID { get; set; }

        public string Id { get; set; }

        public bool? ApprovalStatus { get; set; }

        public DateTime? ApprovalExpiryDT { get; set; }

        public bool StockFeedAccess { get; set; }

        public bool BatchPieceStockAccess { get; set; }

        public bool UpcomingSeasonEarlyAccess { get; set; }

        public ApprovalRequestEditModel()
        {

        }

        public ApprovalRequestEditModel(Flock.Models.ApprovalRequest approvalRequest,
            bool stockFeedAccess,
            bool batchPieceStockAccess,
            bool upcomingSeasonEarlyAccess
            )
        {
            ApprovalRequestID = approvalRequest.ApprovalRequestID;
            Id = approvalRequest.Id;
            ApprovalStatus = approvalRequest.ApprovalStatus;
            ApprovalExpiryDT = approvalRequest.ApprovalExpiryDT;
            StockFeedAccess = stockFeedAccess;
            BatchPieceStockAccess = batchPieceStockAccess;
            UpcomingSeasonEarlyAccess = upcomingSeasonEarlyAccess;
        }
    }
}