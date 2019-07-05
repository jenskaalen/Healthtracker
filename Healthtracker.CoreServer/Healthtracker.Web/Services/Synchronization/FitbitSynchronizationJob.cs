using Healthtracker.Web.Model;
using Healthtracker.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Services.Synchronization
{
    public class FitbitSynchronizationJob : ISynchronizationJob
    {
        private readonly string _accessToken;
        private readonly IFitbitRepository _fitbitRepository;
        private readonly ILogRepository _logRepository;

        public FitbitSynchronizationJob(string userId, string accessToken, IFitbitRepository fitbitRepository, ILogRepository logRepository)
        {
            UserId = userId;
            this._accessToken = accessToken;
            this._fitbitRepository = fitbitRepository;
            this._logRepository = logRepository;
        }

        public string UserId { get; }

        public string Name => "Fitbit";

        public Task Start()
        {
            SynchronizeSleep(_accessToken);
            SynchronizeHeartrates(_accessToken);
            SynchronizeActivities(_accessToken);
            return Task.Delay(500);
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
