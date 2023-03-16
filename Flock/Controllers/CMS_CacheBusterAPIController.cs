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
            if (objCache.Sites.Any())
            {
                foreach (var s in objCache.Sites)
                {
                    Buster.BustWebCache(new PassBust()
                    {
                        target = 3,
                        items = new List<BustItem>()
                        {
                            new BustItem()
                            {
                                templateId = null,
                                explicitExpiry = null,
                                contentDate = DateTime.Now,
                                tradeSwitch = null,
                                contentType = objCache.ContentType,
                                contentId = objCache.ContentID,
                                countryId = s.CountryID,
                                languageId = s.LanguageID
                            }
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
                            target = 3,
                            items = new List<BustItem>()
                            {
                                new BustItem()
                                {
                                    templateId = null,
                                    explicitExpiry = null,
                                    contentDate = DateTime.Now,
                                    tradeSwitch = null,
                                    contentType = objCache.ContentType,
                                    contentId = objCache.ContentID,
                                    countryId = s.CountryID,
                                    languageId = s.LanguageID
                                }
                            }
                        });
                    });
            }

            return Ok(new { });
        }
    }
}
