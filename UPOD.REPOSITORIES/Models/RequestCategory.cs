using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class RequestCategory
    {
        public RequestCategory()
        {
            Requests = new HashSet<Request>();
        }

        public Guid Id { get; set; }
        public string RequestCategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
