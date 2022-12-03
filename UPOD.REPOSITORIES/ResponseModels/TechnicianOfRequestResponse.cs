using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.ResponseViewModel;

namespace UPOD.REPOSITORIES.ResponseModels
{
    public class TechnicianOfRequestResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? technician_name { get; set; }
        public int? number_of_requests { get; set; }
        public string? area { get; set; }
        public List<string>? skills { get; set; }
        public override bool Equals(object? obj)
        {
            TechnicianOfRequestResponse? item = obj as TechnicianOfRequestResponse;

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
