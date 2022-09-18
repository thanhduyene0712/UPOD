using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class RequestUpdateRequest
    {
        public Guid agency_id { get; set; }
        public Guid service_id { get; set; }
        public string request_description { get; set; }
        public string request_name { get; set; }
        public string phone { get; set; }
        public int? estimation { get; set; }
        public int? priority { get; set; }
    }
}
