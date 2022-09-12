using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class AccountRequest
    {
        public string role_name { get; set; }
        public string password { get; set; }
        public bool? is_delete { get; set; }
    }
}
