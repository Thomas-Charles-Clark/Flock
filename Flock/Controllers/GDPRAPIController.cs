using Flock.DAL;
using Flock.Models;
using System.Linq;
using System.Web.Http;

namespace Flock.Controllers
{
    public class passDataSubjectRequest
    {
        public string Email { get; set; }
        public bool RequestUnsubscribe { get; set; }
        public bool RequestDelete { get; set; }
        public bool RequestDataAccess { get; set; }
    }

    public class GDPRAPIController : ApiController
    {
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();
        
        [HttpPost]
        public object DataSubjectRequest(passDataSubjectRequest objRequest)
        {
            int? memberId = null;

            UnitOfWork.Context.DataSubjectRequest_Process(objRequest.Email, objRequest.RequestUnsubscribe, objRequest.RequestDelete, objRequest.RequestDataAccess, ref memberId);

            var results = UnitOfWork.Context.DataSubjectRequest_Select(null).ToList();

            return results;
        }
    }
}