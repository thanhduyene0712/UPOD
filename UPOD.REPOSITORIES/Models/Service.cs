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
            Guidelines = new HashSet<Guideline>();
            Requests = new HashSet<Request>();
            Skills = new HashSet<Skill>();
        }

        public Guid Id { get; set; }
        public Guid AreaId { get; set; }
        public string ServiceName { get; set; }
        public string Desciption { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Area Area { get; set; } = null!;
        public virtual ICollection<ContractService> ContractServices { get; set; }
        public virtual ICollection<DeviceType> DeviceTypes { get; set; }
        public virtual ICollection<Guideline> Guidelines { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
