using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace healthtracker.Model
{
    public class SiteUser
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public List<LogDay> LogDays { get; set; }
    }
}
