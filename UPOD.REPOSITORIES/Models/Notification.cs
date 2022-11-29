using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Notification
    {
        public Guid Id { get; set; }
        public string? NotificationContent { get; set; }
        public Guid? CurrentObject_Id { get; set; }
        public Guid? UserId { get; set; }
        public string? ObjectName { get; set; }
        public bool? isRead { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
