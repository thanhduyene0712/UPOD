using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class MaintenanceReportViewResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? sche_name { get; set; }
        public string? description { get; set; }
        public DateTime? maintain_time { get; set; }
    }
}
