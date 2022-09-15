using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Area
    {
        public Area()
        {
            Services = new HashSet<Service>();
            Technicans = new HashSet<Technican>();
        }

        public Guid Id { get; set; }
        public string AreaName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Technican> Technicans { get; set; }
    }
}
