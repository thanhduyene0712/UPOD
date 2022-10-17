using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class RequestDisableResponse
    {
        public Guid id { get; set; }
        public bool? isDelete { get; set; }
    }
}
