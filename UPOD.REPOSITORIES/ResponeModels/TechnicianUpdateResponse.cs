using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public  class TechnicianUpdateResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public Guid? area_id { get; set; }
        public string? technician_name { get; set; }
        public Guid? account_id { get; set; }
        public string? telephone { get; set; }
        public string? email { get; set; }
        public int? gender { get; set; }
        public string? address { get; set; }
        public double? rating_avg { get; set; }
        public bool? is_busy { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public List<Guid?> service_id { get; set; }
    }
}
