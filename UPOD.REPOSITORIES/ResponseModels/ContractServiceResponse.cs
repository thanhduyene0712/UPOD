using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class ContractServiceResponse
    {
        public Guid id { get; set; }
        public Guid contract_id { get; set; }
        public Guid service_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
