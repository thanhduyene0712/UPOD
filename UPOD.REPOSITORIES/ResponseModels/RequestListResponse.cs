using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class RequestListResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string? request_name { get; set; }
        public string? description { get; set; }
        public string? request_status { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? priority { get; set; }
        public string? reason { get; set; }
        public ContractViewResponse contract { get; set; } = null!;
        public CustomerViewResponse customer { get; set; } = null!;
        public AgencyViewResponse agency { get; set; } = null!;
        public ServiceViewResponse service { get; set; } = null!;
        public TechnicianViewResponse technician { get; set; } = null!;
       
    }
}
