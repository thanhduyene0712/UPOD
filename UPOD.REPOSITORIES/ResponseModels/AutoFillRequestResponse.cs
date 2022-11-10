using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class AutoFillRequestResponse
    {
        public Guid? report_service_id { get; set; }
        public string? request_description { get; set; }
        public CustomerViewResponse customer { get; set; } = null!;
        public AgencyViewResponse agency { get; set; } = null!;
        public ServiceNotInContractViewResponse service { get; set; } = null!;
    }
}
