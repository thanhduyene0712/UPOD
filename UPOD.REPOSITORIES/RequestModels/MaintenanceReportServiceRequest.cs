using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class ListMaintenanceReportServiceRequest
    {
        public List<MaintenanceReportServiceRequest> maintenance_report_service { get; set; }
    }
    public class MaintenanceReportServiceRequest
    {
        public Guid? service_id { get; set; }
        public string? Description { get; set; }
    }
}
