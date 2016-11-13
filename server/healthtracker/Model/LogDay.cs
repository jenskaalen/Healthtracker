using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace healthtracker.Model
{
    public class LogDay
    {
        public List<LogEntry> LogEntries { get; set; } 
    }

    public class LogEntry
    {
        public int Id { get; set; }
        public LogType LogType { get; set; }
        public object Value { get; set; }
    }

    public class LogType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LogDataType DataType { get; set; }
    }

    public enum LogDataType
    {
        Text,
        Select
    }
}
