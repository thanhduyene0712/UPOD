using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class TechnicianOfRequestResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? technician_name { get; set; }
        public int? number_of_requests { get; set; }
    }
}
