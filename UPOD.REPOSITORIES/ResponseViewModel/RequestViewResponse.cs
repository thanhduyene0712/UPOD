using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class RequestViewResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? request_name { get; set; }
    }
}