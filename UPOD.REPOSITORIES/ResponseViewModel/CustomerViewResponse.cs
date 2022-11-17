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
        public string? cus_name { get; set; }
        public string? address { get; set; }
        public string? mail { get; set; }
        public string? phone { get; set; }
        public string? description { get; set; }
    }
}
