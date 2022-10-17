using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class CustomerResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
        public AccountViewResponse account { get; set; } = null!;
        public string? mail { get; set; }
        public string? address { get; set; }
        public string? phone { get; set; }
        public string? description { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
