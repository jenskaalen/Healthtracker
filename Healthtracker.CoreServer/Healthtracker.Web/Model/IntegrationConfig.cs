using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Model
{
    public class IntegrationConfig
    {
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        public string FitbitClientId { get; set; }
        public string FitbitClientBase64 { get; set; }
        public string CertificateName { get; set; }
        public string CertificatePassword { get; set; }
    }
}
