using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class DeviceViewResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? device_name { get; set; }
    }
}
