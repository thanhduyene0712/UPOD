using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Account
    {
        public Account()
        {
            Companies = new HashSet<Company>();
            Technicans = new HashSet<Technican>();
        }

        public Guid Id { get; set; }
        public Guid? RoleId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Technican> Technicans { get; set; }
    }
}
