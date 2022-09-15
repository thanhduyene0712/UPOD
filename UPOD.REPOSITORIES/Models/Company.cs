using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Company
    {
        public Company()
        {
            Agencies = new HashSet<Agency>();
            Contracts = new HashSet<Contract>();
            Devices = new HashSet<Device>();
            Requests = new HashSet<Request>();
        }

        public Guid Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double? PercentForTechnicanExp { get; set; }
        public double? PercentForTechnicanRate { get; set; }
        public double? PercentForTechnicanFamiliarWithAgency { get; set; }
        public bool? IsDelete { get; set; }
        public Guid? AccountId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<Agency> Agencies { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
