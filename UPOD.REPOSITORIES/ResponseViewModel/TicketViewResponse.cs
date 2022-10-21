using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class TicketViewResponse
    {
        public Guid id { get; set; }
        public Guid? device_id { get; set; }
        public string? code { get; set; }
        public string? description { get; set; }
        public string? solution { get; set; }
    }
}
