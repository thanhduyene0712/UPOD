using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class MaintenanceReportServiceResponse
    {
        public Guid id { get; set; }
        public Guid? maintenance_report_id { get; set; }
        public Guid? service_id { get; set; }
        public string? description { get; set; }
    }
}
