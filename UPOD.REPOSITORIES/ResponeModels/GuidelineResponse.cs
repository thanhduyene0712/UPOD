﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class GuidelineResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public ServiceViewResponse service { get; set; }
        public string? guideline_des { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public string? guideline_name { get; set; }
        public DateTime? update_date { get; set; }
    }
}
