using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Agency
    {
        public Agency()
        {
            Devices = new HashSet<Device>();
            Requests = new HashSet<Request>();
        }

        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public string? AgencyName { get; set; }
        public string? Address { get; set; }
        public string? Telephone { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? AreaId { get; set; }
        public string? ManagerName { get; set; }
        public string? Code { get; set; }
        public Guid? TechnicianDefault { get; set; }

        public virtual Area? Area { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
