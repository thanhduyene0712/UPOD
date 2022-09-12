using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class CompanyRequest
    {
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public double? PercentForTechnicanExp { get; set; }
        public double? PercentForTechnicanRate { get; set; }
        public double? PercentForTechnicanFamiliarWithAgency { get; set; }
    }
}
