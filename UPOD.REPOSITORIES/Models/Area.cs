using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Area
    {
        public Area()
        {
            Agencies = new HashSet<Agency>();
            Technicians = new HashSet<Technician>();
        }

        public Guid Id { get; set; }
        public string? AreaName { get; set; }
        public string? Description { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Code { get; set; }

        public virtual ICollection<Agency> Agencies { get; set; }
        public virtual ICollection<Technician> Technicians { get; set; }
    }
}
