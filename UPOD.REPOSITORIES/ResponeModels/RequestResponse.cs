using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.Models;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class RequestResponse
    {
        public Guid id { get; set; }    
        public string agency_name { get; set; }
        public string service_name { get; set; }
        public string company_name { get; set; }
        public int? estimation { get; set; }
        public string request_name { get; set; }
        public int? request_status { get; set; }
    }
}
