using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class ImageResponse
    {
        public Guid? id { get; set; }
        public string? object_name { get; set; }
        public string? link { get; set; }
    }
}
