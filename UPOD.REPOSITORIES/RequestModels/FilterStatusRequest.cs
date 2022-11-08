using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class FilterStatusRequest
    {
        public string? search { get; set; }
        public string? status { get; set; }
    }
}
