using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class MaintenanceSchedule
    {
        public MaintenanceSchedule()
        {
            Tickets = new HashSet<Ticket>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? MaintainTime { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Code { get; set; }
        public Guid? TechnicianId { get; set; }
        public Guid? AgencyId { get; set; }
        public string? Status { get; set; }

        public virtual Agency? Agency { get; set; }
        public virtual Technician? Technician { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
