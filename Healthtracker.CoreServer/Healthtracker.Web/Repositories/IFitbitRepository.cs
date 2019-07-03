using Healthtracker.Web.Model;
using System;
using System.Collections.Generic;

namespace Healthtracker.Web.Repositories
{
    public interface IFitbitRepository
    {
        List<FitbitHeartrate> GetFitbitHeartrates(string token, DateTime fromDate, FitbitTimeSpan timeSpan);
        List<FitbitSleep> GetFitbitSleep(string accessToken, DateTime from, DateTime to, FitbitTimeSpan timeSpan);
        List<FitbitActivity> GetFitbitActivities(string accessToken, DateTime afterDate, int limit, int offset, string sort = "desc");
    }
}
