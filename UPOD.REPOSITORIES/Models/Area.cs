using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Area
    {
        public Area()
        {
            Agencies = new HashSet<Agency>();
            Technicans = new HashSet<Technican>();
        }

        public Guid Id { get; set; }
        public string? AreaName { get; set; }
        public string? Description { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<Agency> Agencies { get; set; }
        public virtual ICollection<Technican> Technicans { get; set; }
    }
}
