using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Account
    {
        public Account()
        {
            Customers = new HashSet<Customer>();
            Technicians = new HashSet<Technician>();
        }

        public Guid Id { get; set; }
        public Guid? RoleId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Code { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Technician> Technicians { get; set; }
    }
}
