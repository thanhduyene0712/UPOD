using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Request
    {
        public Request()
        {
            Tickets = new HashSet<Ticket>();
        }

        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? ServiceId { get; set; }
        public string? RequestDesciption { get; set; }
        public string? RequestStatus { get; set; }
        public string? RequestName { get; set; }
        public string? ReasonReject { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid? CurrentTechnicianId { get; set; }
        public double? Rating { get; set; }
        public string? Feedback { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Priority { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? ExceptionSource { get; set; }
        public string? Img { get; set; }
        public string? Token { get; set; }
        public string? Code { get; set; }
        public Guid? AdminId { get; set; }

        public virtual Admin? Admin { get; set; }
        public virtual Agency? Agency { get; set; }
        public virtual Technician? CurrentTechnician { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Service? Service { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
