using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class CustomerUpdateRequest
    {
            public string? name { get; set; }
            public string? description { get; set; }
            public string? address { get; set; }
            public string? mail { get; set; }
            public string? phone { get; set; }
    }
}
