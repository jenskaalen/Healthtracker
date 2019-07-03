using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Healthtracker.Web.Model;
using Healthtracker.Web.Repositories;
using Healthtracker.Web.Services;
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
        private readonly IntegrationConfig config;

        private string UserId => User.Identity.Name;

        public FitbitController(IHttpClientFactory clientFactory, IFitbitRepository fitbitRepository, ILogRepository logRepository, IMemoryCache memoryCache, FitbitTokenStorage fitbitTokenStorage, IOptions<IntegrationConfig> config)
        {
            _clientFactory = clientFactory;
            this._fitbitRepository = fitbitRepository;
            this._logRepository = logRepository;
            this.memoryCache = memoryCache;
            this.fitbitTokenStorage = fitbitTokenStorage;
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
            postbody.Add("redirect_uri", "https://localhost:44354/signin-fitbit");

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
                SynchronizeActivities(accessToken);
                return LocalRedirect("/");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void SynchronizeActivities(string accessToken)
        {
            int yearsModifier = -3;
            int limit = 20;
            int offset = 0;
            List<FitbitActivity> activityData = _fitbitRepository.GetFitbitActivities(accessToken, DateTime.Today.AddYears(yearsModifier), limit, offset);
            List<Model.Log> logs = _logRepository.GetAll(UserId);

            foreach (var log in logs)
            {
                var activitiesMatchingDate = activityData.Where(act => act.OriginalStartTime.Date == log.Date.Date);
                log.FitbitActivities = activitiesMatchingDate.ToList();
                _logRepository.Update(log);
            }
        }

        private void SynchronizeSleep(string accessToken)
        {
            List<Model.FitbitSleep> sleepData = _fitbitRepository.GetFitbitSleep(accessToken, DateTime.Today.AddDays(-30), DateTime.Today, FitbitTimeSpan.Month);
            List<Model.Log> logs = _logRepository.GetAll(UserId);

            foreach (var sleep in sleepData)
            {
                IEnumerable<Model.Log> matchingLogs = logs.Where(log => log.Date.Date == sleep.Date.Date);

                matchingLogs.ToList().ForEach(log =>
                {
                    log.Sleep = sleep.Sleep;
                    _logRepository.Update(log);
                });
            }
        }

        private void SynchronizeHeartrates(string accessToken)
        {
            List<Model.FitbitHeartrate> heartrates = _fitbitRepository.GetFitbitHeartrates(accessToken, DateTime.Today, FitbitTimeSpan.Month);
            List<Model.Log> logs = _logRepository.GetAll(UserId);

            foreach (var heartRate in heartrates)
            {
                IEnumerable<Model.Log> matchingLogs = logs.Where(log => log.Date.Date == heartRate.Date.Date);

                matchingLogs.ToList().ForEach(log =>
                {
                    log.RestingHeartrate = heartRate.RestingHeartrate;
                    _logRepository.Update(log);
                });
            }
            
        }
    }
}