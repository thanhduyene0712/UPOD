using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Agencies = new HashSet<Agency>();
            Contracts = new HashSet<Contract>();
            MaintenanceReports = new HashSet<MaintenanceReport>();
            Requests = new HashSet<Request>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsDelete { get; set; }
        public Guid? AccountId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? Mail { get; set; }
        public string? Phone { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<Agency> Agencies { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<MaintenanceReport> MaintenanceReports { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
