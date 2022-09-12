using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class ServiceRequest
    {
        public Guid dep_id { get; set; }
        public string service_name { get; set; }
        public string? desciption { get; set; }
    }
}
