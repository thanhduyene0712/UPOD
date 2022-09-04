using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class AccRegisterRespone
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string Username { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
