using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class TechnicianViewResponse
    {
        public Guid? id  { get; set; }
        public string? code { get; set; }
        public string? tech_name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
    }
}
