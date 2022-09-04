using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class AccRegisterRequest
    {
        public string RoleName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
