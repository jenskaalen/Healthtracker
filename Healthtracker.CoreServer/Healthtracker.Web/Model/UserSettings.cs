using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Model
{
    public class UserSettings
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<string> DailySupplements { get; set; }
    }
}
