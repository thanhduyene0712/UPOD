using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class CompanyRespone
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public double? PercentForTechnicanExp { get; set; }
        public double? PercentForTechnicanRate { get; set; }
        public double? PercentForTechnicanFamiliarWithAgency { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
