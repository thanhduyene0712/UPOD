using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Account
    {
        public Account()
        {
            Agencies = new HashSet<Agency>();
            Technicans = new HashSet<Technican>();
        }

        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Agency> Agencies { get; set; }
        public virtual ICollection<Technican> Technicans { get; set; }
    }
}
