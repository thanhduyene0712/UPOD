using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Contract
    {
        public Contract()
        {
            ContractServices = new HashSet<ContractService>();
        }

        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string ContractName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public double? ContractPrice { get; set; }
        public DateTime? TimeCommit { get; set; }
        public int? Priority { get; set; }
        public string PunishmentForCustomer { get; set; }
        public string PunishmentForIt { get; set; }
        public string Desciption { get; set; }

        public virtual Company Company { get; set; } = null!;
        public virtual ICollection<ContractService> ContractServices { get; set; }
    }
}
