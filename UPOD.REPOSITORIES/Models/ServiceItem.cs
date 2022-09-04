using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class ServiceItem
    {
        public ServiceItem()
        {
            Guidelines = new HashSet<Guideline>();
            Requests = new HashSet<Request>();
        }

        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public string GuidelineName { get; set; } = null!;
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Service Service { get; set; } = null!;
        public virtual ICollection<Guideline> Guidelines { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
