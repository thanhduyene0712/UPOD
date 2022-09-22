using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class GuidelineRequest
    {
        public Guid? service_id { get; set; }
        public string? guideline_des { get; set; }
        public string? guideline_name { get; set; }
    }
}
