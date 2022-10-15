using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class ChangePasswordRequest
    {
        public string? old_password { get; set; }

        public string? new_password { get; set; }
        public string? confirm_password { get; set; }

    }
}
