using System;

namespace Healthtracker.Web.Model
{
    public class FitbitActivity
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public int ActiveDuration { get; set; }
        //public double ActiveMinutes => ActiveDuration / 1000 / 60;
        public string ActivityName { get; set; }
        public DateTime OriginalStartTime { get; set; }
    }
}