using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class RequestHistory
    {
        public Guid Id { get; set; }
        public Guid? RequestId { get; set; }
        public Guid? PreTechnicanId { get; set; }
        public int? PreStatus { get; set; }
        public bool? IsTechnicanAccept { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Description { get; set; }

        public virtual Technican? PreTechnican { get; set; }
        public virtual Request? Request { get; set; }
    }
}
