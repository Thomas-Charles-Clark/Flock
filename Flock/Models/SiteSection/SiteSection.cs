using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;

namespace Flock.Models
{
    public class SiteSectionModel
    {
        public string Name { get; set; }

        public long SiteSectionID { get; set; }

        public static IEnumerable<SiteSection> GetSections()
        {
            Flock.Models.FlockDataContext db = new Flock.Models.FlockDataContext();

            List<SiteSection> result = new List<SiteSection>();

            ISingleResult<SiteSection_SelectResult> sub = db.SiteSection_Select();
            foreach(SiteSection_SelectResult val in sub)
            {
                SiteSection ss = new SiteSection();
                ss.Name = val.Name;
                ss.SiteSectionID = val.SiteSectionID;
                result.Add(ss);
            }

            return result;
        }
    }
}