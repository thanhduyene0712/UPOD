using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Skill
    {
        public Guid Id { get; set; }
        public Guid? TechnicianId { get; set; }
        public Guid? ServiceId { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Code { get; set; }

        public virtual Service? Service { get; set; }
        public virtual Technician? Technician { get; set; }
    }
}
