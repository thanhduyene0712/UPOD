using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class AdminResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
        public string? mail { get; set; }
        public string? telephone { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public Guid? account_id { get; set; }
    }
}
