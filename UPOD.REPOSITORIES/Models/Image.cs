using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Image
    {
        public Guid Id { get; set; }
        public string? Link { get; set; }
        public Guid? ObjectId { get; set; }

        public virtual Admin? Object { get; set; }
        public virtual Contract? Object1 { get; set; }
        public virtual Customer? Object2 { get; set; }
        public virtual Device? Object3 { get; set; }
        public virtual MaintenanceReportService? Object4 { get; set; }
        public virtual Request? Object5 { get; set; }
        public virtual Service? Object6 { get; set; }
        public virtual Technician? Object7 { get; set; }
        public virtual Ticket? Object8 { get; set; }
        public virtual Agency? ObjectNavigation { get; set; }
    }
}
