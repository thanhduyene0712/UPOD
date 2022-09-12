using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class CompanyRequest
    {
        public string company_name { get; set; }
        public string description { get; set; }
        public double? percent_for_technican_exp { get; set; }
        public double? percent_for_technican_rate { get; set; }
        public double? percent_for_technican_familiar_with_agency { get; set; }
    }
}
