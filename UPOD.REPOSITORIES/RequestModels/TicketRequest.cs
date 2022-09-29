using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class TicketRequests
    {
        public List<TicketRequest> tickets { get; set; }
    }
    public class TicketRequest
    {
        public Guid? device_id { get; set; }
        public string? description { get; set; }
        public string? solution { get; set; }
    }
}
