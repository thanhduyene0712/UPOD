using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class ServiceRespone
    {
        public Guid Id { get; set; }
        public Guid DepId { get; set; }
        public string ServiceName { get; set; }
        public string? Desciption { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
