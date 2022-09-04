using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class DepartmentItmapping
    {
        public Guid Id { get; set; }
        public Guid TechnicanId { get; set; }
        public Guid DepId { get; set; }
        public bool? IsDelete { get; set; }

        public virtual Department Dep { get; set; } = null!;
        public virtual Technican Technican { get; set; } = null!;
    }
}
