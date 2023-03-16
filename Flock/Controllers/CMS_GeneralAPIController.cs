using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Flock.Controllers
{
    public class CMS_GeneralAPIController : ApiController
    {
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        [HttpPost]
        public IHttpActionResult SavePagePodAttributes([FromBody] PassPagePodSave objSave)
        {
            // Was having problems with calls to both endpoints matching,
            // so... if PageSectionID == 0, we're actually handling a Canonical save.
            if (objSave.PageSectionID != 0)
            {
                var pps = UnitOfWork.GetRepository<PageSection_Page>().GetAll().Single(p => p.PageSectionID == objSave.PageSectionID &&
                p.PageID == objSave.PageID);

                pps.AttributeList = JsonConvert.SerializeObject(objSave.AttributeIDs);

                UnitOfWork.Save();
            }
            else
            {
                var page = UnitOfWork.GetRepository<Page>().GetAll().Single(p => p.PageID == objSave.PageID);

                page.CanonicalAttributeList = JsonConvert.SerializeObject(objSave.AttributeIDs);

                UnitOfWork.Save();

            }

            return Ok("");
        }

    }

    public class CMS_CanonicalAPIController : ApiController
    {
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        [HttpPost]
        public IHttpActionResult SaveCanonicalPageAttributes([FromBody]PassCanonicalPageAttributeSave objSave)
        {
            var page = UnitOfWork.GetRepository<Page>().GetAll().Single(p => p.PageID == objSave.PageID);

            page.CanonicalPageID = objSave.DestinationPageID;

            if (objSave.AttributeIDs != null && 
                objSave.AttributeIDs.Any())
            {
                page.CanonicalAttributeList = JsonConvert.SerializeObject(objSave.AttributeIDs);
            }
            else
            {
                page.CanonicalAttributeList = null;
            }

            UnitOfWork.Save();

            return Ok("");
        }
    }
}