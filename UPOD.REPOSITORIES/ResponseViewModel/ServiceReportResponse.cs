using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class ServiceReportResponse
    {

        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? service_name { get; set; }
        public string? description { get; set; }
        public bool? created { get; set; }


    }
}
