using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Service
    {
        public Service()
        {
            ContractServices = new HashSet<ContractService>();
            DeviceTypes = new HashSet<DeviceType>();
            ServiceItems = new HashSet<ServiceItem>();
            Skills = new HashSet<Skill>();
        }

        public Guid Id { get; set; }
        public Guid DepId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string? Desciption { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Department Dep { get; set; } = null!;
        public virtual ICollection<ContractService> ContractServices { get; set; }
        public virtual ICollection<DeviceType> DeviceTypes { get; set; }
        public virtual ICollection<ServiceItem> ServiceItems { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
