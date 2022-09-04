using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class RequestTask
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public string? TaskDetails { get; set; }
        public int? TaskStatus { get; set; }
        public string CreateByTechnican { get; set; } = null!;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Priority { get; set; }
        public int? PreTaskCondition { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Request Request { get; set; } = null!;
    }
}
