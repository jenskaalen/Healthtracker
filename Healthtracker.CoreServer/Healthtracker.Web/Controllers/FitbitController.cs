using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Healthtracker.Web.Model;
using Healthtracker.Web.Repositories;
using Healthtracker.Web.Services;
using Healthtracker.Web.Services.Synchronization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Healthtracker.Web.Controllers
{
    
    [ApiController]
    public class FitbitController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IFitbitRepository _fitbitRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMemoryCache memoryCache;
        private readonly FitbitTokenStorage fitbitTokenStorage;
        private readonly ISyncQueue syncQueue;
        private readonly IntegrationConfig config;

        private string UserId => User.Identity.Name;
        

        public FitbitController(IHttpClientFactory clientFactory, IFitbitRepository fitbitRepository, ILogRepository logRepository, IMemoryCache memoryCache, FitbitTokenStorage fitbitTokenStorage, IOptions<IntegrationConfig> config, ISyncQueue syncQueue)
        {
            _clientFactory = clientFactory;
            this._fitbitRepository = fitbitRepository;
            this._logRepository = logRepository;
            this.memoryCache = memoryCache;
            this.fitbitTokenStorage = fitbitTokenStorage;
            this.syncQueue = syncQueue;
            this.config = config.Value;
        }

        [Route("signin-fitbit")]
        [HttpGet]
        public ActionResult GetTokenAsync(string code)
        {
            string url = "https://api.fitbit.com/oauth2/token";
            var request = new HttpRequestMessage(HttpMethod.Post,
                url);
            var postbody = new Dictionary<string, string>();
            postbody.Add("clientId", config.FitbitClientId);
            postbody.Add("grant_type", "authorization_code");
            postbody.Add("code", code);

            string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";
            postbody.Add("redirect_uri", $"{baseUrl}/signin-fitbit");

            request.Headers.Add("Authorization", $"Basic { config.FitbitClientBase64}");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            request.Headers.Add("ContentType", "application/x-www-form-urlencoded");
            request.Content = new FormUrlEncodedContent(postbody);

            //post
            HttpClient client = _clientFactory.CreateClient();
            HttpResponseMessage response = client.SendAsync(request).Result;
                
            string content = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                string accessToken = fitbitTokenStorage.GetAccessTokenFromJson(content);
                string refreshToken = fitbitTokenStorage.GetRefreshTokenFromJson(content);
                var expires = DateTime.Now.AddDays(1);
                fitbitTokenStorage.StoreAccessToken(accessToken, expires, UserId);

                //SynchronizeHeartrates(accessToken);
                //SynchronizeSleep(accessToken);
                //SynchronizeActivities(accessToken);
                var job = new FitbitSynchronizationJob(UserId, accessToken, _fitbitRepository, _logRepository);
                syncQueue.Add(job);

                return LocalRedirect("/");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}