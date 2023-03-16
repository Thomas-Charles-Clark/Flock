using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock
{
    public struct SortDatum
    {
        public long ID;
        public short SortOrder;
    }

    public struct CountryLanguageSortDatum
    {
        public short CountryID;
        public byte LanguageID;
        public short SortOrder;
    }

    public struct PageSectionContentsInformation
    {
        public string DataType;
        public long ContentID;
        public byte SortOrder;
    }
}

namespace Flock.Util
{
    public static class Utilities
    {
        public static string DetermineApprovalStatus(bool? approvalStatus, DateTime? expiry)
        {
            if (approvalStatus == null)
            {
                return "Not Approved";
            }
            else if (approvalStatus == false)
            {
                if (DateTime.Now < expiry)
                    return "Timed Approval";
                else
                    return "Approval Expired";
            }
            else if (approvalStatus == true)
            {
                return "Indefinitely Approved";
            }
        
            return "";
        }
    }
}