using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.RequestModels;

namespace UPOD.REPOSITORIES.ResponseModels
{
    
    public class ServiceResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? service_name { get; set; }
        public string? description { get; set; }
        public bool? is_delete { get; set; }
        public string? guideline { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
