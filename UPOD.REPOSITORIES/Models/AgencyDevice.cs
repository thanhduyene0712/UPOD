using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.Models
{
    public class AgencyDevice
    {
        public Guid? DeviceId { get; set; }
        public Guid? AgencyId { get; set; }

        public Agency? Agency { get; set; }
        public Device? Device { get; set; }

    }
}
