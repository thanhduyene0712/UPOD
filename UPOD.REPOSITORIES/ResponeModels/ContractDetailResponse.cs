using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class ContractDetailResponse
    {
        public string company_name { get; set; }
        public string contract_name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? create_date { get; set; }
        public double? contract_price { get; set; }
        public DateTime? time_commit { get; set; }
        public int? priority { get; set; }
        public string punishment_for_customer { get; set; }
        public string punishment_for_it { get; set; }
        public string desciption { get; set; }
    }
}
