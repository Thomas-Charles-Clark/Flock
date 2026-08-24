using CacheBuster;
using CacheBuster.Models;
using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Flock.Controllers
{
    public class CMS_CacheBusterAPIController : ApiController
    {
        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        [HttpPost]
        public object BustCache([FromBody]PassCacheBuster objCache)
        {
            // only relevant to landing pages
            var pageSectionIds = new long[] { };

            if (objCache.ContentType == "landingPage")
            {
                pageSectionIds = UoW.GetRepository<PageSection>().GetAll().Where(ps => ps.PageID == objCache.ContentID && ps.Deleted == false)
                    .Select(ps => ps.PageSectionID)
                    .ToArray();
            }

            if (objCache.Sites.Any())
            {
                foreach (var s in objCache.Sites)
                {
                    Buster.BustWebCache(new PassBust()
                    {
#if DEBUG
                        host = "www.designersguild.com",
#else
                        host = "localhost:44338",
#endif
                        items = new List<BustItem>()
                        {
                            new BustItem(
                                templateId: null,
                                explicitExpiry: null,
                                contentDate: DateTime.Now,
                                tradeSwitch: null,
                                contentType: objCache.ContentType,
                                contentId: objCache.ContentID,
                                countryId: s.CountryID,
                                languageId: s.LanguageID,
                                pageSectionIds: pageSectionIds
                            )
                        }
                    });
                }
            }
            else
            {
                UoW.GetRepository<Country_Language>().GetAll().Where(p => UoW.Context.fnc_DateControl(p.OnDate, p.OffDate, DateTime.Now) == true)
                    .ToList()
                    .ForEach(s =>
                    {
                        Buster.BustWebCache(new PassBust()
                        {
#if DEBUG
                        host = "www.designersguild.com",
#else
                        host = "localhost:44338",
#endif
                            items = new List<BustItem>()
                            {
                                new BustItem(
                                    templateId: null,
                                    explicitExpiry: null,
                                    contentDate: DateTime.Now,
                                    tradeSwitch: null,
                                    contentType: objCache.ContentType,
                                    contentId: objCache.ContentID,
                                    countryId: s.CountryID,
                                    languageId: s.LanguageID,
                                    pageSectionIds: pageSectionIds
                                )
                            }
                        });
                    });
            }

            return Ok(new { });
        }
    }
}
