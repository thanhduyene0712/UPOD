using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class MappingTechnicianResponse
    {
        public Guid id { get; set; }
        public Guid technician_id { get; set; }
        public string? request_status { get; set; }
        public DateTime? start_time { get; set; }                 
    }
}
