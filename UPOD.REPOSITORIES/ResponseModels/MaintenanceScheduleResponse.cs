﻿using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class MaintenanceScheduleResponse
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public DateTime? maintain_time { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string? code { get; set; }
        public TechnicianViewResponse technician { get; set; } = null!;
        public AgencyViewResponse agency { get; set; } = null!;
        public string? status { get; set; }
    }
}