﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class DeviceTypeResponse
    {
        public Guid id { get; set; }
        public Guid? service_id { get; set; }
        public string? device_type_name { get; set; }
        public string? description { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}