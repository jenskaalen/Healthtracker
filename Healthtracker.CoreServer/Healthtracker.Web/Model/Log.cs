using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Model
{
    public class Log
    {
        public int Id { get; set; }
        public string DocumentId { get ; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Activity { get; set; }
        public string Comment { get; set; }
        public int Feeling { get; set; }

    }
}
