using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class DeviceUpdateRequest
    {
        public Guid? devicetype_id { get; set; }
        public string? device_name { get; set; }
        public DateTime? guaranty_start_date { get; set; }
        public DateTime? guaranty_end_date { get; set; }
        public string? ip { get; set; }
        public int? port { get; set; }
        public string? device_account { get; set; }
        public string? device_password { get; set; }
        public DateTime? setting_date { get; set; }
        public string? other { get; set; }
    }
}
