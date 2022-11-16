using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Device
    {
        public Device()
        {
            Images = new HashSet<Image>();
            Tickets = new HashSet<Ticket>();
        }

        public Guid Id { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid? DeviceTypeId { get; set; }
        public string? DeviceName { get; set; }
        public string? Code { get; set; }
        public DateTime? GuarantyStartDate { get; set; }
        public DateTime? GuarantyEndDate { get; set; }
        public string? Ip { get; set; }
        public int? Port { get; set; }
        public string? DeviceAccount { get; set; }
        public string? DevicePassword { get; set; }
        public DateTime? SettingDate { get; set; }
        public string? Other { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? CreateBy { get; set; }

        public virtual Agency? Agency { get; set; }
        public virtual DeviceType? DeviceType { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
