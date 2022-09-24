using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public Guid Id { get; set; }
        public string? RoleName { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Code { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
