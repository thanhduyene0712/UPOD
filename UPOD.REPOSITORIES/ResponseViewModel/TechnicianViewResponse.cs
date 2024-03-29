﻿namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class TechnicianViewResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? tech_name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public override bool Equals(object? obj)
        {
            TechnicianViewResponse? item = obj as TechnicianViewResponse;

            if (item == null)
            {
                return false;
            }

            return item.id == this.id;
        }
        public override int GetHashCode()
        {
            return this.id.GetHashCode() * 25;
        }
    }
}
