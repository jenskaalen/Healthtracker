using Healthtracker.Web.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Healthtracker.Web.Repositories
{
    public class FitbitRepository : IFitbitRepository
    {
        private readonly IHttpClientFactory clientFactory;

        public FitbitRepository(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public List<FitbitHeartrate> GetFitbitHeartrates(string token, DateTime fromDate, FitbitTimeSpan timeSpan)
        {
            string dateFormatted = fromDate.ToString("yyyy-MM-dd");
            string timespanText = ToText(timeSpan);

            string url = "https://api.fitbit.com/1/user/-/activities/heart/date/today/1m.json";
            var request = new HttpRequestMessage(HttpMethod.Get,
                url);
            var postbody = new Dictionary<string, string>();
            //TODO: get client ID from settings

            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            request.Headers.Add("ContentType", "application/x-www-form-urlencoded");
            request.Content = new FormUrlEncodedContent(postbody);

            HttpClient client = clientFactory.CreateClient();
            HttpResponseMessage response = client.SendAsync(request).Result;

            string jsonContent = response.Content.ReadAsStringAsync().Result;


            return GetFitbitHeartrates(jsonContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json">Valid json from fitbit</param>
        /// <returns></returns>
        public List<FitbitHeartrate> GetFitbitHeartrates(string json)
        {
            JObject jbobject = JObject.Parse(json);
            var obj = jbobject["activities-heart"];
            var heartrates = new List<FitbitHeartrate>();

            obj.Children().ToList().ForEach(day =>
            {
                try
                {
                    DateTime date = DateTime.Parse((string)day["dateTime"]);
                    int restingHeartrate = (int)day["value"]["restingHeartRate"];

                    heartrates.Add(new FitbitHeartrate
                    {
                        Date = date,
                        RestingHeartrate = restingHeartrate
                    });
                }
                catch (Exception ex)
                {
                    //TODO: do something?
                }
            });

            return heartrates;
        }

        private string ToText(FitbitTimeSpan timeSpan)
        {
            switch (timeSpan)
            {
                case FitbitTimeSpan.Thirtydays:
                    return "30d";
                    
                case FitbitTimeSpan.Month:
                    return "1m";

                default:
                    throw new NotImplementedException();
            }
        }

        public List<FitbitSleep> GetFitbitSleep(string accessToken, DateTime from, DateTime to, FitbitTimeSpan timeSpan)
        {
            string fromDateFormatted = from.ToString("yyyy-MM-dd");
            string toDateFormatted = to.ToString("yyyy-MM-dd");
            string toTimespanFormatted = ToText(timeSpan);

            string url = $"https://api.fitbit.com/1.2/user/-/sleep/date/{fromDateFormatted}/{toDateFormatted}.json";
            var request = new HttpRequestMessage(HttpMethod.Get,
                url);
            var postbody = new Dictionary<string, string>();
            //TODO: get client ID from settings

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            request.Headers.Add("ContentType", "application/x-www-form-urlencoded");
            request.Content = new FormUrlEncodedContent(postbody);

            HttpClient client = clientFactory.CreateClient();
            HttpResponseMessage response = client.SendAsync(request).Result;

            string jsonContent = response.Content.ReadAsStringAsync().Result;
            
            return GetFitbitSleep(jsonContent);
        }

        public List<FitbitSleep> GetFitbitSleep(string json)
        {
            JObject jbobject = JObject.Parse(json);
            var obj = jbobject["sleep"];
            var sleepData = new List<FitbitSleep>();

            obj.Children().ToList().ForEach(day =>
            {
                try
                {
                    DateTime date = DateTime.Parse((string)day["dateOfSleep"]);
                    int sleepMinutes = (int)day["minutesAsleep"];

                    sleepData.Add(new FitbitSleep
                    {
                        Date = date,
                        Sleep = sleepMinutes / 60.0m
                    });
                }
                catch (Exception ex)
                {
                }
            });

            return sleepData;
        }

        public List<FitbitActivity> GetFitbitActivities(string accessToken, DateTime afterDate, int limit, int offset, string sort = "desc")
        {
            string afterDateFormatted = afterDate.ToString("yyyy-MM-dd");

            string url = $"https://api.fitbit.com/1/user/-/activities/list.json?afterDate={afterDateFormatted}&sort={sort}&offset={offset}&limit={limit}";
            HttpRequestMessage request = CreateRequest(accessToken, url);

            HttpClient client = clientFactory.CreateClient();
            HttpResponseMessage response = client.SendAsync(request).Result;

            string jsonContent = response.Content.ReadAsStringAsync().Result;

            return GetActivitiesFromJson(jsonContent);
        }

        private static HttpRequestMessage CreateRequest(string accessToken, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                url);

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            request.Headers.Add("ContentType", "application/x-www-form-urlencoded");
            return request;
        }

        private List<FitbitActivity> GetActivitiesFromJson(string json)
        {
            JObject jbobject = JObject.Parse(json);

            JEnumerable<JToken> jActivities = jbobject["activities"].Children();
            var activities = new List<FitbitActivity>();

            jActivities.ToList().ForEach(jActivity =>
            {
                var fitbitActivity = JsonConvert.DeserializeObject<FitbitActivity>(jActivity.ToString(Formatting.None));
                activities.Add(fitbitActivity);
            });

            return activities;
        }
    }
}
