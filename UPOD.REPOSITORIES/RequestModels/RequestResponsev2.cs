using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class RequestResponsev2
    {
        public Guid id { get; set; }
        public Guid company_id { get; set; }
        public Guid service_id { get; set; }
        public string request_desciption { get; set; }
        public int? request_status { get; set; }
        public string request_name { get; set; }
        public int? estimation { get; set; }
        public string phone { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public Guid agency_id { get; set; }
        public Guid? current_technican_id { get; set; }
        public double? rating { get; set; }
        public string feedback { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public int? priority { get; set; }
        public DateTime? update_date { get; set; }
        public string exception_source { get; set; }
        public string solution { get; set; }
        public string img { get; set; }
        public string token { get; set; }
    }
}
