using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.RequestModels
{
    public class AccountUpdateRequest
    {
        public Guid role_id { get; set; }
        public string? password { get; set; }
    }
}
