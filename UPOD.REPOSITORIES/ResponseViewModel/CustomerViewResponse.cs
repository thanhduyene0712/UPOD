using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class CustomerViewResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public double? percent_for_technican_exp { get; set; }
        public double? percent_for_technican_rate { get; set; }
        public double? percent_for_technican_familiar_with_agency { get; set; }
    }
}
