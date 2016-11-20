using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace healthtracker.Model
{
    public class LogDay
    {
        public long Id { get; set; }
        public List<LogEntry> LogEntries { get; set; } 
        public DateTime? Registered { get; set; }
    }
}
