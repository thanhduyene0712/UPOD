using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Guideline
    {
        public Guid Id { get; set; }
        public Guid? ServiceId { get; set; }
        public string? Guideline1 { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? GuidelineName { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Service? Service { get; set; }
    }
}
