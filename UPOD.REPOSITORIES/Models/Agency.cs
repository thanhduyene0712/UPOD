using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Agency
    {
        public Agency()
        {
            Requests = new HashSet<Request>();
        }

        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string AgencyName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ManagerName { get; set; }

        public virtual Company Company { get; set; } = null!;
        public virtual ICollection<Request> Requests { get; set; }
    }
}
