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

namespace Flock.Classes.SmartFocus
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
    public partial class SmartFocusManager
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private const string _apiCCmdServer = "https://p4apic.emv2.com/apiccmd/services/rest/";
        public string CampaignCommandServer
        {
            get { return _apiCCmdServer; }
        }

        private const string _apiBatchMemberServer = "https://p4apic.emv2.com/apibatchmember/services/rest/";
        public string BatchMemberServer
        {
            get { return _apiBatchMemberServer; }
        }

        private const string _apiMemberServer = "https://p4apic.emv2.com/apimember/services/rest/";
        public string MemberServer
        {
            get { return _apiMemberServer; }
        }

        private const string _apiTransactionalServer = "https://p4apic.emv2.com/apitransactional/services/rest/";
        public string TransactionalTemplateServer
        {
            get { return _apiTransactionalServer; }
        }


        private const string _login = "API_designers_TC";
        private const string _pwd = "Rpl94mjsdV3@";
        private const string _key = "CdX7CrVn3XiwlUVOWcUhvoiURygJJtm2mUGqBdxUO6-uLk-fj7i81Npnnw";

        public SmartFocusManager() { }

        /// <summary>
        /// Opens a new connection to the SmartFocus REST API
        /// </summary>
        /// <returns>The ConnectionToken for the created connection.</returns>
        public async Task<string> OpenConnection(string server = _apiCCmdServer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(server);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("connect/open/{0}/{1}/{2}", _login, _pwd, _key)).ConfigureAwait(false);

                BasicResponse connectResponse = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                return connectResponse.Result;
            }
        }

        /// <summary>
        /// Closes a connection to the SmartFocus REST API
        /// </summary>
        /// <param name="connectionToken">The ConnectionToken to close.</param>
        /// <returns>Result of close - hopefully 'connection closed'.</returns>
        public async Task<string> CloseConnection(string connectionToken, string server = _apiCCmdServer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(server);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("connect/close/{0}", connectionToken)).ConfigureAwait(false);

                BasicResponse connectResponse = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                return connectResponse.Result;
            }
        }

        /// <summary>
        /// Retrieves complete list of Segments
        /// 
        /// Can't yet find a way to determine the total number of segments, so can't see how to page. 
        /// follows this format: /list/[pageNumber]/items/[pageLimit]
        /// 
        /// </summary>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>List Flock.Classes.SmartFocus.Segment</returns>
        public async Task<List<Segment>> GetSegmentList(string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                
                HttpResponseMessage response = await client.GetAsync(String.Format("segmentationservice/{0}/segment/list/1/items/100", connectionToken)).ConfigureAwait(false);
                EntityListResponse IDs = await response.Content.ReadAsAsync<EntityListResponse>().ConfigureAwait(false);

                List<Segment> SegmentList = new List<Segment>();

                foreach(long l in IDs.Entities)
                {
                    SegmentList.Add(await GetSegment(l, connectionToken).ConfigureAwait(false));
                }

                if(_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return SegmentList;
            }
        }

        /// <summary>
        /// Retrieves information regarding a Member Segment
        /// </summary>
        /// <param name="Id">SegmentID to retrieve information about.</param>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>Flock.Classes.SmartFocus.Segment</returns>
        public async Task<Segment> GetSegment(long Id, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("segmentationservice/{0}/segment/{1}", connectionToken, Id)).ConfigureAwait(false);
                SegmentResponse s = await response.Content.ReadAsAsync<SegmentResponse>().ConfigureAwait(false);

                //s.Segment.CountDistinctMembers = await GetSegmentDistinctMemberCount(s.Segment.ID).ConfigureAwait(false);

                if(_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return s.Segment;
            }
        }

        public async Task<long> GetSegmentDistinctMemberCount(long segmentId)
        {
            string connectionToken = await OpenConnection().ConfigureAwait(false);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("segmentationservice/{0}/segment/{1}/distinctCount", connectionToken, segmentId)).ConfigureAwait(false);
                BasicResponse s = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                
                await CloseConnection(connectionToken).ConfigureAwait(false);

                return Convert.ToInt64(s.Result);
            }
        }

        public async Task<string> CreateSegment(string segmentName, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                var xmlContents = String.Format(@"
                    <segmentation>
                        <description>TEST SEGMENT</description>
                        <name>{0}</name>
                        <sampleType>ALL</sampleType>
                    </segmentation>", segmentName);

                var putContent = new StringContent(xmlContents, System.Text.Encoding.UTF8, "text/xml");

                HttpResponseMessage response = await client.PutAsync(String.Format("segmentationservice/{0}/segment", connectionToken), putContent).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);


                if (_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return Response.Result;
            }
        }

        /*
         <stringDemographicCriteria>
<groupName>GROUP 4</groupName>
<groupNumber>4</groupNumber>
<columnName>EMAIL</columnName>
<operator>CONTAINS</operator>
<values>hotmail</values>
</stringDemographicCriteria>
         */

        /// <summary>
        /// This was used to add the criteria: EMAIL CONTAINS test.com 
        /// Then modified to try to update to current.
        /// Gets a 405 - Method Not Allowed.
        /// </summary>
        /// <param name="_connectionToken"></param>
        /// <returns></returns>
        public async Task<string> AddTestCriteriaToTestAPISegment(string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            //https://{server}/apiccmd/services/rest/

            List<long> DemographicIDs = new List<long>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("segmentationservice/{0}/segment/1604519/criteria/list", connectionToken)).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                var xmlContents = String.Format(@"
<stringDemographicCriteria>
<groupName>GROUP1</groupName>
<groupNumber>1</groupNumber>
<columnName>EMAIL</columnName>
<operator>ENDS_WITH</operator>
<values>.test.com</values>
</stringDemographicCriteria>");

                var putContent = new StringContent(xmlContents, System.Text.Encoding.UTF8, "text/xml");

                HttpResponseMessage response = await client.PostAsync(String.Format("segmentationservice/{0}/segment/1604519/criteria/0/updateStringDemographic", connectionToken), putContent).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return Response.Result;
            }
        }

        /// <summary>
        /// Fetches all TestGroupIDs
        /// </summary>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>Flock.Classes.SmartFocus.EntityListResponse</returns>
        public async Task<EntityListResponse> FetchTestGroupIDs(string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("testgroup/getAll/{0}", connectionToken)).ConfigureAwait(false);
                EntityListResponse Groups = await response.Content.ReadAsAsync<EntityListResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return Groups;
            }
        }

        /// <summary>
        /// Fetches TestGroup
        /// </summary>
        /// <param name="groupId">GroupID to fetch</param>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>Flock.Classes.SmartFocus.TestGroup</returns>
        public async Task<TestGroup> FetchTestGroup(long groupId, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("testgroup/get/{0}/{1}", connectionToken, groupId)).ConfigureAwait(false);
                TestGroupResponse TestGroupResponse = await response.Content.ReadAsAsync<TestGroupResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return TestGroupResponse.Group;
            }
        }

        /// <summary>
        /// Creates a new TestGroup
        /// </summary>
        /// <param name="name">Name of the TestGroup to create</param>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>ID of TestGroup created</returns>
        public async Task<string> CreateTestGroup(string name, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("testgroup/create/{0}/{1}", connectionToken, name)).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return Response.Result;
            }
        }

        /// <summary>
        /// Creates a new Test Member
        /// </summary>
        /// <param name="email">Email to for new Test Member</param>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>Flock.Classes.SmartFocus.TestMember</returns>
        public async Task<TestMember> CreateTestMember(string email, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("testgroup/createTestMember/{0}/{1}", connectionToken, email)).ConfigureAwait(false);
                TestMemberResponse Response = await response.Content.ReadAsAsync<TestMemberResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return Response.TestMember;
            }
        }


        /// <summary>
        /// Fetches Member by Email
        /// </summary>
        /// <param name="email">Email to Fetch on</param>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>Flock.Classes.SmartFocus.TestMember</returns>
        public async Task<string> FetchMemberByEmail(string email, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiMemberServer).ConfigureAwait(false) : _connectionToken;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiMemberServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("member/getMemberByEmail/{0}/{1}", connectionToken, email)).ConfigureAwait(false);
                string Response = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiMemberServer).ConfigureAwait(false);

                return Response;
            }
        }

        /// <summary>
        /// Fetches TestMember 
        /// </summary>
        /// <param name="memberId">MemberID to fetch</param>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>Flock.Classes.SmartFocus.TestMember</returns>
        public async Task<TestMember> GetTestMember(long memberId, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("testgroup/getTestMember/{0}/{1}", connectionToken, memberId)).ConfigureAwait(false);
                TestMemberResponse Response = await response.Content.ReadAsAsync<TestMemberResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return Response.TestMember;
            }
        }

        /// <summary>
        /// Retrieves all TestMembers
        /// </summary>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>List Flock.Classes.SmartFocus.TestMember</returns>
        public async Task<List<TestMember>> GetAllTestMembers(string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("testgroup/getTestMembers/{0}", connectionToken)).ConfigureAwait(false);
                ListTestMembersResponse Response = await response.Content.ReadAsAsync<ListTestMembersResponse>().ConfigureAwait(false);

                List<TestMember> ResultList = new List<TestMember>();

                foreach(long l in Response.TestMembers)
                {
                    ResultList.Add(await GetTestMember(l,  connectionToken).ConfigureAwait(false));
                }

                if (_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return ResultList;
            }
        }

        /// <summary>
        /// Adds a Test Member to a Test Group
        /// </summary>
        /// <param name="memberID">MemberID to add to Group</param>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns></returns>
        public async Task<string> AddTestMemberToTestGroup(long memberID, long groupId, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection().ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("testgroup/addTestMember/{0}/{1}/{2}", connectionToken, memberID, groupId)).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken).ConfigureAwait(false);

                return Response.Result;
            }
        }


        public async Task<string> UploadAndMerge(string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiBatchMemberServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
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
            }
        }

        ////////////////////////////////////
        //
        //  Still required:
        //      Unsubscribe Member
        //      Resubscribe Member
        //      Update Member (primarily Email address)
        //

        public async Task<string> UnsubscribeMemberByEmail(string _email, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiMemberServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("member/unjoinByEmail/{0}/{1}", connectionToken, _email)).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiMemberServer).ConfigureAwait(false);

                return Response.Result;
            }
        }

        public async Task<string> ResubscribeMemberByEmail(string _email, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiMemberServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiCCmdServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("member/rejoinByEmail/{0}/{1}", connectionToken, _email)).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiMemberServer).ConfigureAwait(false);

                return Response.Result;
            }
        }

        public async Task<string> UpdateMemberEmailByEmail(string _currentEmail, string _newEmail, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiMemberServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiMemberServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("member/update/{0}/{1}/EMAIL/{2}", connectionToken, _currentEmail, _newEmail)).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiMemberServer).ConfigureAwait(false);

                return Response.Result;
            }
        }
    }

#region Data Contracts

    [DataContract(Name = "response", Namespace = "")]
    public class SegmentResponse
    {
        [DataMember(Name = "segmentation")]
        public Segment Segment;
    }

    [DataContract(Name = "segmentation", Namespace = "")]
    public class Segment
    {
        [DataMember(Name = "dateCreate")]
        public DateTime CreatedDT;

        [DataMember(Name = "dateModif")]
        public DateTime? ModifiedDT;

        [DataMember(Name = "id")]
        public long ID;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "sampleRate")]
        public decimal SampleRate;

        [DataMember(Name = "sampleType")]
        public string SampleType;

        public long CountDistinctMembers;
    }

    [DataContract(Name = "response", Namespace = "")]
    public class ListTestMembersResponse
    {
        [DataMember(Name = "testMembers")]
        public TestMemberList TestMembers;
    }

    [DataContract(Name = "response", Namespace = "")]
    public class TestMemberResponse
    {
        [DataMember(Name = "member")]
        public TestMember TestMember;
    }

    [DataContract(Name = "member", Namespace = "")]
    public class TestMember
    {
        [DataMember(Name = "clientId")]
        public long ClientID;

        [DataMember(Name = "dateJoin")]
        public DateTime DateJoined;

        [DataMember(Name = "email")]
        public string Email;

        [DataMember(Name = "id")]
        public long ID;
    }

    [DataContract(Name = "response", Namespace = "")]
    public class BasicResponse
    {
        [DataMember(Name = "result")]
        public string Result { get; set; }
    }

    [DataContract(Name = "response", Namespace = "")]
    public class EntityListResponse
    {
        [DataMember(Name = "entities")]
        public EntityList Entities;
    }

    [CollectionDataContract(ItemName = "id", Namespace = "")]
    public class EntityList : List<long> {}

    [DataContract(Name = "response", Namespace = "")]
    public class TestGroupResponse
    {
        [DataMember(Name = "testGroup")]
        public TestGroup Group;
    }

    [DataContract(Name = "testGroup", Namespace = "")]
    public class TestGroup
    {
        [DataMember(Name = "id")]
        public string ID;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "testMembers")]
        public TestMemberList Members;
    }

    [CollectionDataContract(ItemName = "memberId", Namespace = "")]
    public class TestMemberList : List<long> {}

    public class TestGroupCollection
    {
        public EntityListResponse TestGroupIDs { get; set; }

        public List<TestGroup> Groups { get; set; }
    }
#endregion
}