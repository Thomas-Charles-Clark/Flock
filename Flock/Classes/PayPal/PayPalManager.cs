using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Web;
using NLog;
using System.Text;
using System.Xml;
using System.IO;

using PayPal.Api;
using PayPal.OAuth;
using PayPal.Authentication;
using PayPal.Manager;

namespace Flock.Classes.PayPal
{
    /// <summary>
    /// 
    ///     Manages connecting to and consuming the SmartFocus REST API
    ///     
    ///     API Login information must be updated when expired...
    /// 
    /// 
    ///     *************************************************************************************************************
    ///     *   N.B.
    ///     *       ASP.NET MVC5 does not support calling asynchronous methods from child ActionResults
    ///     *       this means, essentially, that the functions are not called asynch.
    ///     *       This is why all the awaits you see in this class are .ConfigureAwait(false) - to prevent blocking.
    ///     *
    ///     *       One may call these functions in an asynchronous manner, but only from a top-level ActionResult 
    ///     *       which returns a full View. Partials MUST call async Task<T> functions fetching .Result
    ///     *
    ///     *       Implementation Examples to follow
    /// 
    /// 
    /// </summary>
    public partial class PayPalContext
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private const string _server = "https://api.sandbox.paypal.com/";
        public string Server
        {
            get { return _server; }
        }

        private const string _login = "paypal-facilitator@designersguild.com";
        private const string _clientId = "AY3VpRBB2bLus0i_khNXt84ec_aOtFS-v3FQW_GlTsY_C-5rZS4ua5_4Oj-t@";
        private const string _key = "EOls4RBYDzQu_aQKsJ9XEIUGLb0hbpYhSuNBHzsS-Ngwf6SbGY0kKTMLkDhN";

        public PayPalContext() 
        { 
            
        }

        /// <summary>
        /// Opens a new connection to the PayPal REST API
        /// </summary>
        /// <returns>The ConnectionToken for the created connection.</returns>
        public async Task<string> FetchToken(string server = _server)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(server);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("accept-language/en_US"));

                HttpResponseMessage response = await client.GetAsync(String.Format("v1/oauth2/token/?u="+_clientId+":"+_key+"&grant_type=client_credentials")).ConfigureAwait(false);

                string connectResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return connectResponse;
            }
        }
        /*
                client.BaseAddress = new Uri(_apiBatchMemberServer);
                client.DefaultRequestHeaders.Accept.Clear();

                string boundary = Guid.NewGuid().ToString();

                MultipartFormDataContent content = new MultipartFormDataContent(boundary);
                content.Headers.Clear();
                content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);

                string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<mergeUpload>\r\n<fileName>member.csv</fileName>\r\n<fileEncoding>UTF-8</fileEncoding>\r\n<separator>,</separator>\r\n"
                    + "<dateFormat>mm/dd/yyyy</dateFormat>\r\n"
                    + "<criteria>LOWER(EMAIL)</criteria>\r\n"
                    + "<mapping>\r\n<column>\r\n<colNum>1</colNum>\r\n<fieldName>EMAIL</fieldName>\r\n</column>\r\n"
                    + "</mapping>\r\n</mergeUpload>";

                var xmlContent = new StringContent(xmlString);
                xmlContent.Headers.Clear();
                xmlContent.Headers.TryAddWithoutValidation("Content-Disposition", "form-data; name=\"mergeUpload\"");
                xmlContent.Headers.TryAddWithoutValidation("Content-Type", "text/xml");
                content.Add(xmlContent);

                string base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes("C:\\smartfocusintegration\\TestMembersUpload.csv"));
                byte[] stringBytes = Encoding.ASCII.GetBytes(base64String);

                var base64Content = new ByteArrayContent(stringBytes);

                base64Content.Headers.Clear();
                base64Content.Headers.TryAddWithoutValidation("Content-Disposition", @"form-data; name=""inputStream""");
                base64Content.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                base64Content.Headers.TryAddWithoutValidation("Content-Transfer-Encoding", "base64");
                base64Content.Headers.Remove("Content-Length");

                content.Add(base64Content);

                content.Headers.Remove("Content-Length");
                content.ElementAt(0).Headers.Remove("Content-Length");
                content.ElementAt(1).Headers.Remove("Content-Length");

                HttpResponseMessage response = await client.PutAsync(String.Format("batchmemberservice/{0}/batchmember/mergeUpload", connectionToken), content).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiBatchMemberServer).ConfigureAwait(false);

                return Response.Result;
            }*/
    }

    #region Data Contracts

    //[DataContract(Name = "response", Namespace = "")]
    //public class SegmentResponse
    //{
    //    [DataMember(Name = "segmentation")]
    //    public Segment Segment;
    //}

    //[DataContract(Name = "segmentation", Namespace = "")]
    //public class Segment
    //{
    //    [DataMember(Name = "dateCreate")]
    //    public DateTime CreatedDT;

    //    [DataMember(Name = "dateModif")]
    //    public DateTime? ModifiedDT;

    //    [DataMember(Name = "id")]
    //    public long ID;

    //    [DataMember(Name = "name")]
    //    public string Name;

    //    [DataMember(Name = "sampleRate")]
    //    public decimal SampleRate;

    //    [DataMember(Name = "sampleType")]
    //    public string SampleType;

    //    public long CountDistinctMembers;
    //}

    //[DataContract(Name = "response", Namespace = "")]
    //public class ListTestMembersResponse
    //{
    //    [DataMember(Name = "testMembers")]
    //    public TestMemberList TestMembers;
    //}

    //[DataContract(Name = "response", Namespace = "")]
    //public class TestMemberResponse
    //{
    //    [DataMember(Name = "member")]
    //    public TestMember TestMember;
    //}

    //[DataContract(Name = "member", Namespace = "")]
    //public class TestMember
    //{
    //    [DataMember(Name = "clientId")]
    //    public long ClientID;

    //    [DataMember(Name = "dateJoin")]
    //    public DateTime DateJoined;

    //    [DataMember(Name = "email")]
    //    public string Email;

    //    [DataMember(Name = "id")]
    //    public long ID;
    //}

    //[DataContract(Name = "response", Namespace = "")]
    //public class BasicResponse
    //{
    //    [DataMember(Name = "result")]
    //    public string Result { get; set; }
    //}

    //[DataContract(Name = "response", Namespace = "")]
    //public class EntityListResponse
    //{
    //    [DataMember(Name = "entities")]
    //    public EntityList Entities;
    //}

    //[CollectionDataContract(ItemName = "id", Namespace = "")]
    //public class EntityList : List<long> { }

    //[DataContract(Name = "response", Namespace = "")]
    //public class TestGroupResponse
    //{
    //    [DataMember(Name = "testGroup")]
    //    public TestGroup Group;
    //}

    //[DataContract(Name = "testGroup", Namespace = "")]
    //public class TestGroup
    //{
    //    [DataMember(Name = "id")]
    //    public string ID;

    //    [DataMember(Name = "name")]
    //    public string Name;

    //    [DataMember(Name = "testMembers")]
    //    public TestMemberList Members;
    //}

    //[CollectionDataContract(ItemName = "memberId", Namespace = "")]
    //public class TestMemberList : List<long> { }

    //public class TestGroupCollection
    //{
    //    public EntityListResponse TestGroupIDs { get; set; }

    //    public List<TestGroup> Groups { get; set; }
    //}
    #endregion
}