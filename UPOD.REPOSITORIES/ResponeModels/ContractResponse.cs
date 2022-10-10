using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class ContractResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string? contract_name { get; set; }
        public CustomerViewResponse customer { get; set; } = null!;
        public double? contract_price { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? priority { get; set; }
        public string? attachment { get; set; }
        public string? img { get; set; }
        public string? description { get; set; }
        public int? frequency_maintain { get; set; }
        public List<ServiceViewResponse> service { get; set; } = null!;
    }
}
