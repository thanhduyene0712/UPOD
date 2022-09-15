using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class CompanyResponse
    {
        public Guid id { get; set; }
        public string company_name { get; set; }
        public string description { get; set; }
        public double? percent_for_technican_exp { get; set; }
        public double? percent_for_technican_rate { get; set; }
        public double? percent_for_technican_familiar_with_agency { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
