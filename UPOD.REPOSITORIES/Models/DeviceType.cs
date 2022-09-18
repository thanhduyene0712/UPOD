using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class DeviceType
    {
        public DeviceType()
        {
            Devices = new HashSet<Device>();
        }

        public Guid Id { get; set; }
        public Guid? ServiceId { get; set; }
        public string? DeviceTypeName { get; set; }
        public string? Desciption { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Service? Service { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}
