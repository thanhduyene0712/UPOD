using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class TechnicianUpdateRequest
    {
            public Guid? area_id { get; set; }
            public string? technician_name { get; set; }
            public string? telephone { get; set; }
            public string? email { get; set; }
            public int? gender { get; set; }
            public string? address { get; set; }
            public List<Guid?> service_id { get; set; } = null!;
    }
}
