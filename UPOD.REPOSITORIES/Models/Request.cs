using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Request
    {
        public Request()
        {
            RequestHistories = new HashSet<RequestHistory>();
            Tickets = new HashSet<Ticket>();
        }

        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid ServiceId { get; set; }
        public string? RequestDesciption { get; set; }
        public Guid RequestCategoryId { get; set; }
        public int? RequestStatus { get; set; }
        public string RequestName { get; set; } = null!;
        public int? Estimation { get; set; }
        public string? Phone { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid CurrentTechnicanId { get; set; }
        public double? Rating { get; set; }
        public string? Feedback { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Priority { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? ExceptionSource { get; set; }
        public string? Solution { get; set; }
        public string? Img { get; set; }
        public string? Token { get; set; }

        public virtual Agency? Agency { get; set; }
        public virtual Company Company { get; set; } = null!;
        public virtual Technican CurrentTechnican { get; set; } = null!;
        public virtual Service Service { get; set; } = null!;
        public virtual ICollection<RequestHistory> RequestHistories { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
