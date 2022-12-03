using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class ContractRequest
    {
        public Guid customer_id { get; set; }
        public string? contract_name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public double? contract_price { get; set; }
        public string? attachment { get; set; }
        public string? description { get; set; }
        public int? frequency_maintain_time { get; set; }
        public List<string>? img { get; set; }
        public List<ServiceFrequencyRequest> service { get; set; } = null!;

    }
}
