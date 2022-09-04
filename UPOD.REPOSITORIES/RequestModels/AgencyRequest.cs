using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class AgencyRequest
    {
        public string CompanyName { get; set; }
        public string Username { get; set; }
        public string AgencyName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string ManagerName { get; set; }
    }
}
