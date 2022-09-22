using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class RequestCreateResponse
    {
        public string request_name { get; set; }
        public string request_description { get; set; }
        public int? estimation { get; set; }
        public string phone { get; set; }
        public int? priority { get; set; }
        public string agency_name { get; set; }
        public string service_name { get; set; }
    }
}
