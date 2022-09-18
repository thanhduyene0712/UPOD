using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class ContractService
    {
        public Guid Id { get; set; }
        public Guid? ContactId { get; set; }
        public Guid? ServiceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Contract? Contact { get; set; }
        public virtual Service? Service { get; set; }
    }
}
