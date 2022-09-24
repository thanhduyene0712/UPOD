using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class ContractListResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string? customer_name { get; set; }
        public string? contract_name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? create_date { get; set; }
    }
}
