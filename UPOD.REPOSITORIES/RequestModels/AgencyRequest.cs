using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class AgencyRequest
    {
        public Guid? customer_id { get; set; }
        public Guid? technician_default { get; set; }
        public string? agency_name { get; set; }
        public string? address { get; set; }
        public string? telephone { get; set; }
        public Guid? area_id { get; set; }
        public string? manager_name { get; set; }
    }
}
