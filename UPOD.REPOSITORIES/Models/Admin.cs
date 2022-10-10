using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Admin
    {
        public Admin()
        {
            Requests = new HashSet<Request>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Mail { get; set; }
        public string? Telephone { get; set; }
        public bool? IsDelete { get; set; }
        public string? CreateDate { get; set; }
        public string? UpdateDate { get; set; }
        public Guid? AccountId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
