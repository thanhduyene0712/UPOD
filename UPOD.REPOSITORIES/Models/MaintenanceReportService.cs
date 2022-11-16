﻿using System;
using System.Collections.Generic;

namespace UPOD.REPOSITORIES.Models
{
    public partial class MaintenanceReportService
    {
        public MaintenanceReportService()
        {
            Images = new HashSet<Image>();
        }

        public Guid Id { get; set; }
        public Guid? MaintenanceReportId { get; set; }
        public Guid? ServiceId { get; set; }
        public string? Description { get; set; }
        public bool? Created { get; set; }

        public virtual MaintenanceReport? MaintenanceReport { get; set; }
        public virtual Service? Service { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
