using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class RequestDetailResponse
    {
        public string agency_name { get; set; }
        public string address_service { get; set; }
        public string service_name { get; set; }
        public string description_serivce { get; set; }
        public string company_name { get; set; }
        public int? estimation { get; set; }
        public string request_name { get; set; }
        public int? request_status { get; set; }
        public int? priority { get; set; }
        public string phone { get; set; }
        public string description_request { get; set; }                                   

    }
}
