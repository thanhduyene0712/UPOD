using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class AgencyResponse
    {
        public Guid id { get; set; }
        public Guid? company_id { get; set; }
        public string? agency_name { get; set; }
        public string? address { get; set; }
        public string? telephone { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public Guid? area_id { get; set; }
        public string? manager_name { get; set; }
        public List<Guid?> device_id { get; set; }

    }
}
