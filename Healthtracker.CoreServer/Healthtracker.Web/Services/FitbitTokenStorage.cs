
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Services
{
    public class FitbitTokenStorage
    {
        private readonly IMemoryCache cache;

        public FitbitTokenStorage(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void StoreAccessToken(string accessToken, DateTime expires, string userId)
        {
            string cacheName = $"{userId}-fitbit-access-token";
            cache.Set(cacheName, accessToken, expires);
        }

        public string GetRefreshTokenFromJson(string content)
        {
            var obj = JObject.Parse(content);
            return obj["refresh_token"].ToString();
        }

        public string GetAccessTokenFromJson(string content)
        {
            var obj = JObject.Parse(content);
            return obj["access_token"].ToString();
        }
    }
}
