using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Healthtracker.Web.Model
{
    public class Log
    {
        private string _documentId;

        public int Id { get; set; }

        //NOTE: not really a fan of this
        public string DocumentId { get => _documentId; set
            {
                _documentId = value;
                Id = int.Parse(Regex.Match(_documentId, @"\d+").Value);
            }
        }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Activity { get; set; }
        public string Comment { get; set; }
        public int Feeling { get; set; }

    }
}
