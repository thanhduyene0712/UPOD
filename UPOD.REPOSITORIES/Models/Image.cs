using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Image
    {
        public Guid Id { get; set; }
        public string? Link { get; set; }
        public Guid? CurrentObject_Id { get; set; }
    }
}
