using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Service.CommunicationStandard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utilities.NewtonsoftSerializer;

namespace Website.MarketingSite.Services
{
    public class HttpServiceBase
    {
        public HttpClient Client { get; }

        public HttpServiceBase(HttpClient client)
        {
            Client = client;
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
        }        

        protected virtual HttpRequestMessage InitRequest(HttpMethod method, string path, Dictionary<string, string> queries = null)
        {
            ValidateRequest(path);

            string requestUri = path;

            if (queries != null)
            {
                var listQuery = queries.Select(s => string.Format("{0}={1}", s.Key, s.Value));
                var queryString = string.Join("&", listQuery);
                requestUri = string.Format("{0}?{1}", requestUri, queryString);
            }

            HttpRequestMessage request = new HttpRequestMessage(method, requestUri);

            return request;
        }

        protected virtual void SetRequestBody(HttpRequestMessage requestMessage, object obj)
        {
            if (obj != null)
            {
                var serialized = JsonConvert.SerializeObject(obj, NewtonJsonSerializerSettings.CAMEL);
                requestMessage.Content = new StringContent(serialized, Encoding.UTF8, "application/json");
            }
        }

        protected virtual void SetRequestId(HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.Add("x-requestid", Guid.NewGuid().ToString());
        }

        protected virtual void SetAuthorizationJwt(HttpRequestMessage requestMessage, string authJwt)
        {
            requestMessage.Headers.Add("Authorization", $"Bearer {authJwt}");
        }

        protected async Task<ActionResultModel> PostActionAsync(string path, object obj = null, Dictionary<string, string> queries = null)
        {
            var response = await PostAsync(path, obj, queries);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                string data = obj != null ? JsonConvert.SerializeObject(obj) : "";
                string queryStr = queries != null ? JsonConvert.SerializeObject(queries) : "";
                throw new Exception($"Internal server error at {path}\nQuery: {queryStr}\nData: {data}");
            }

            var raw = await response.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ActionResultModel>(raw);

            return apiResult;
        }

        protected void ValidateRequest(string path)
        {
            if (path == null)
            {
                throw new Exception("uri is null");
            }
        }

        protected void SetRequestHeaders(HttpRequestMessage requestMessage, bool includeRequestId, string jwt)
        {
            if (includeRequestId)
            {
                SetRequestId(requestMessage);
            }

            if (!string.IsNullOrEmpty(jwt))
            {
                SetAuthorizationJwt(requestMessage, jwt);
            }
        }

        protected async Task<HttpResponseMessage> GetAsync(string path, Dictionary<string, string> queries = null, bool includeRequestId = false, string jwt = null)
        {
            var requestMessage = InitRequest(HttpMethod.Get, path, queries);
            SetRequestHeaders(requestMessage, includeRequestId, jwt);
            return await Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> PostAsync(string path, object obj, Dictionary<string, string> queries = null, bool includeRequestId = true, string jwt = null)
        {
            var requestMessage = InitRequest(HttpMethod.Post, path, queries);

            SetRequestBody(requestMessage, obj);
            SetRequestHeaders(requestMessage, includeRequestId, jwt);

            return await Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> PutAsync(string path, object obj, Dictionary<string, string> queries = null, bool includeRequestId = true, string jwt = null)
        {
            var requestMessage = InitRequest(HttpMethod.Put, path, queries);

            SetRequestBody(requestMessage, obj);
            SetRequestHeaders(requestMessage, includeRequestId, jwt);

            return await Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string path, object obj = null, Dictionary<string, string> queries = null, bool includeRequestId = true, string jwt = null)
        {
            var requestMessage = InitRequest(HttpMethod.Delete, path, queries);

            SetRequestBody(requestMessage, obj);
            SetRequestHeaders(requestMessage, includeRequestId, jwt);

            return await Client.SendAsync(requestMessage);
        }
    }
}
