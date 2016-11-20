namespace healthtracker.Model
{
    public class LogEntry
    {
        public long Id { get; set; }
        public LogType LogType { get; set; }
        public string TextValue { get; set; }
        public int NumberValue { get; set; }
    }
}