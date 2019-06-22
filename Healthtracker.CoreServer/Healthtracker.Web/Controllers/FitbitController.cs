﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Healthtracker.Web.Repositories;
using Healthtracker.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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

        private string UserId => User.Identity.Name;

        public FitbitController(IHttpClientFactory clientFactory, IFitbitRepository fitbitRepository, ILogRepository logRepository, IMemoryCache memoryCache, FitbitTokenStorage fitbitTokenStorage)
        {
            _clientFactory = clientFactory;
            this._fitbitRepository = fitbitRepository;
            this._logRepository = logRepository;
            this.memoryCache = memoryCache;
            this.fitbitTokenStorage = fitbitTokenStorage;
        }

        [Route("signin-fitbit")]
        [HttpGet]
        public void GetTokenAsync(string code)
        {
            string url = "https://api.fitbit.com/oauth2/token";
            var request = new HttpRequestMessage(HttpMethod.Post,
                url);
            var postbody = new Dictionary<string, string>();
            //TODO: get client ID from settings
            postbody.Add("clientId", "22DR79");
            postbody.Add("grant_type", "authorization_code");
            postbody.Add("code", code);
            postbody.Add("redirect_uri", "https://localhost:44354/signin-fitbit");

            request.Headers.Add("Authorization", "Basic MjJEUjc5OjRhYWZiMmQwZDRiNWRhYWNhNjg3Zjc5YmU0Y2FlNWVh");
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

                SynchronizeHeartrates(accessToken);
                SynchronizeSleep(accessToken);
                LocalRedirect("/");
            }
            else
            {
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