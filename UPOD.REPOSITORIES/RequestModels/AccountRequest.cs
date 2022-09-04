using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class AccountRequest
    {
        public string RoleName { get; set; }
        public string Password { get; set; }
        public bool? IsDelete { get; set; }
    }
}
