using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtilityTools
{
    public class Log
    {
        public int Id { get; set; }

        /// <summary>
        /// Required for ravenDB
        /// </summary>
        public string DocumentId => $"logs/{Id}";
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Activity { get; set; }
        public string Comment { get; set; }
        public int Feeling { get; set; }

    }
}
