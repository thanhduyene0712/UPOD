using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Guideline
    {
        public Guid Id { get; set; }
        public Guid ServiceItemId { get; set; }
        public string? Guideline1 { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ServiceItem ServiceItem { get; set; } = null!;
    }
}
