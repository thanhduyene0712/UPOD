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
        PENDING,
        REJECT,
        PREPARING,
        RESOLVED
    }
}
