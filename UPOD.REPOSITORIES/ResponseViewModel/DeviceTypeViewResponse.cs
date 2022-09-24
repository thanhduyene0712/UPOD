using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class DeviceTypeViewResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public Guid? service_id { get; set; }
        public string? device_type_name { get; set; }
    }
}
