using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class AreaViewResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? area_name { get; set; }
        public string? description { get; set; }
    }
}
