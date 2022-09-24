using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class TicketResponse

    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public Guid? request_id { get; set; } = null!;
        public Guid? device_id { get; set; } = null!;
        public string? description { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string? solution { get; set; }
        public Guid? create_by { get; set; } = null!;
    }
}
