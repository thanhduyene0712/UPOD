using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class AgencyResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string? agency_name { get; set; }
        public CustomerViewResponse customer { get; set; } = null!;
        public AreaViewResponse area { get; set; } = null!;
        public TechnicianViewResponse technician { get; set; }
        public string? address { get; set; }
        public string? telephone { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string? manager_name { get; set; }
        public List<DeviceViewResponse> device { get; set; } = null!;

    }
}
