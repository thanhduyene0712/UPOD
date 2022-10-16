using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class AdminRequest
    {
        public string? name { get; set; }
        public string? mail { get; set; }
        public string? telephone { get; set; }
        public Guid? account_id { get; set; }
    }
}
