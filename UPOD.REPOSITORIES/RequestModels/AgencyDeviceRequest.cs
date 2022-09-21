using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class AgencyDeviceRequest
    {
        public Guid? device_id { get; set; }
        public Guid? agency_id { get; set; }
    }
}
