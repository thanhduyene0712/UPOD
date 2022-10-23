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
        RESOLVING,
        RESOLVED,
        CANCEL
    }

    public enum ReportStatus
    {
        //1:có service hư, 2: không có service hư (nên có 2 status này)


    }
    public enum ScheduleStatus
    {
       SCHEDULED,
       MISSING,
       MAINTAINING, 
       COMPLETED,
       NOT_COMPLETED,
    }
}
