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
using System.ComponentModel.DataAnnotations;

namespace Flock.Classes.SmartFocus
{
    public partial class SmartFocusManager
    {

        public async Task<string> CreateTransactionalTemplate(SFTransactionalTemplate item, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiTransactionalServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiTransactionalServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("template/create/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}",
                    connectionToken, item.Name, item.Description, item.Subject, item.FromName, item.FromEmail,
                    item.ToName, item.Encoding, item.ReplyToName, item.ReplyToEmail, item.Type)).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiTransactionalServer).ConfigureAwait(false);

                return Response.Result;
            }
        }

        public async Task<string> UpdateTemplateField(int templateID, string field, string value, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiTransactionalServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiTransactionalServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("template/update/{0}/{1}/{2}/{3}",
                    connectionToken, templateID, field, value)).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiTransactionalServer).ConfigureAwait(false);

                return Response.Result;
            }
        }

        /// <summary>
        /// Retrieves List of Template
        /// </summary>
        /// <param name="Id">SegmentID to retrieve information about.</param>
        /// <param name="_connectionToken">ConnectionToken to utilise. If null, a new connection is made and closed by this method.</param>
        /// <returns>Flock.Classes.SmartFocus.Segment</returns>
        public async Task<List<APITemplate>> GetLastTransactionalTemplates(string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiTransactionalServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiTransactionalServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("template/getLastTemplates/{0}/TRANSACTIONAL/1000", connectionToken)).ConfigureAwait(false);
                EntityListResponse ids = await response.Content.ReadAsAsync<EntityListResponse>().ConfigureAwait(false);

                List<APITemplate> Templates = new List<APITemplate>();

                foreach(long l in ids.Entities)
                {
                    Templates.Add(await GetTemplate(l, connectionToken).ConfigureAwait(false));
                }
                
                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiTransactionalServer).ConfigureAwait(false);

                return Templates;
            }
        }

        public async Task<APITemplate> GetTemplate(long id, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiTransactionalServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiTransactionalServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("template/get/{0}/{1}", connectionToken, id)).ConfigureAwait(false);
                TemplateResponse template = await response.Content.ReadAsAsync<TemplateResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiTransactionalServer).ConfigureAwait(false);

                return template.Template;
            }
        }


        public async Task<string> CreateDynamicContentBlock(SFDynamicContentBlock item, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiTransactionalServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiTransactionalServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("banner/create/{0}/{1}/{2}/{3}",
                    connectionToken, item.Name, item.Description, item.ContentType)).ConfigureAwait(false);
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiTransactionalServer).ConfigureAwait(false);

                return Response.Result;
            }
        }

        public async Task<List<DynamicContentBlock>> GetLastDynamicContentBlocks(string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiTransactionalServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiTransactionalServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("banner/getLastBanners/{0}/1000", connectionToken)).ConfigureAwait(false);
                EntityListResponse ids = await response.Content.ReadAsAsync<EntityListResponse>().ConfigureAwait(false);

                List<DynamicContentBlock> Blocks = new List<DynamicContentBlock>();

                foreach (long l in ids.Entities)
                {
                    Blocks.Add(await GetDynamicContentBlock(l, connectionToken).ConfigureAwait(false));
                }

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiTransactionalServer).ConfigureAwait(false);

                return Blocks;
            }
        }

        public async Task<DynamicContentBlock> GetDynamicContentBlock(long id, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiTransactionalServer).ConfigureAwait(false) : _connectionToken;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiTransactionalServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync(String.Format("banner/get/{0}/{1}", connectionToken, id)).ConfigureAwait(false);
                DynamicContentBlockResponse template = await response.Content.ReadAsAsync<DynamicContentBlockResponse>().ConfigureAwait(false);

                if (_connectionToken == null)
                    await CloseConnection(connectionToken, _apiTransactionalServer).ConfigureAwait(false);

                return template.Block;
            }
        }

        public async Task<string> AddDynamicContentBlockToTemplate(long id, long dcbID, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiTransactionalServer).ConfigureAwait(false) : _connectionToken;

            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(_apiTransactionalServer);
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await Client.GetAsync(String.Format("template/addBanner/{0}/{1}/{2}", connectionToken, id, dcbID));
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                return Response.Result;
            }
        }


        public async Task<string> SendTestEmailToMember(long id, long memberId, string _connectionToken = null)
        {
            string connectionToken = _connectionToken == null ? await OpenConnection(_apiTransactionalServer).ConfigureAwait(false) : _connectionToken;

            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(_apiTransactionalServer);
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await Client.GetAsync(String.Format("template/testByMember/{0}/{1}/{2}", connectionToken, id, memberId));
                BasicResponse Response = await response.Content.ReadAsAsync<BasicResponse>().ConfigureAwait(false);

                return Response.Result;
            }
        }


    }

    [DataContract(Name = "response", Namespace = "")]
    public class TemplateResponse
    {
        [DataMember(Name = "apiTemplate")]
        public APITemplate Template;
    }

    [DataContract(Name = "apiTemplate", Namespace = "")]
    public class APITemplate
    {
        [DataMember(Name = "body")]
        public string Body;

        [DataMember(Name = "encoding")]
        public string Encoding;

        [DataMember(Name = "encrypt")]
        public string Encrypt;

        [DataMember(Name = "from")]
        public string From;

        [DataMember(Name = "fromEmail")]
        public string Email;

        [DataMember(Name = "id")]
        public long ID;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "random")]
        public string Random;

        [DataMember(Name = "replyToEmail")]
        public string ReplyAddress;

        [DataMember(Name = "sent")]
        public bool Sent;

        [DataMember(Name = "type")]
        public string Type;
    }


    [DataContract(Name = "response", Namespace = "")]
    public class DynamicContentBlockResponse
    {
        [DataMember(Name = "apiBanner")]
        public DynamicContentBlock Block;
    }

    [DataContract(Name = "apiBanner", Namespace = "")]
    public class DynamicContentBlock
    {
        [DataMember(Name = "content")]
        public string Content;

        [DataMember(Name = "contentType")]
        public string ContentType;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "id")]
        public long ID;

        [DataMember(Name = "name")]
        public string Name;
    }

    public class SFTransactionalTemplate
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [StringLength(100)]
        public string FromName { get; set; }

        [Required]
        [StringLength(100)]
        public string FromEmail { get; set; }

        [Required]
        [StringLength(100)]
        public string ToName { get; set; }

        public string Encoding { get; set; }

        [StringLength(100)]
        public string ReplyToName { get; set; }

        [Required]
        [StringLength(100)]
        public string ReplyToEmail { get; set; }

        public string Type { get; set; }
    }

    public class SFDynamicContentBlock
    {
        [Required]
        [StringLength(50, MinimumLength=3)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string ContentType { get; set; }
    }
}