using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public  class DevicesOfRequestResponse
    {
        public Guid ticket_id { get; set; }
        public Guid? device_id { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
        public string? solution { get; set; }
        public string? description { get; set; }
        public DateTime? create_date { get; set; }
        public List<string>? img { get; set; }
    }
}
