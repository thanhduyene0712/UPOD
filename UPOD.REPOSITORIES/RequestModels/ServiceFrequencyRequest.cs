using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    
    public class ServiceFrequencyRequest
    {
        public int? frequency_maintain { get; set; }
        public Guid? service_id { get; set; } = null!;
    }
}
