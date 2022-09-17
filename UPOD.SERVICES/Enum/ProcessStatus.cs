using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.SERVICES.Enum
{
    public enum ProcessStatus
    {
        Pending = 1,
        Reject = 2,
        Preparing = 3,
        Resoved = 4,
        Closed = 5,
    }
}
