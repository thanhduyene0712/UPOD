using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class ServiceNotInContractViewResponse
    {
            public Guid? id { get; set; }
            public string? code { get; set; }
            public string? service_name { get; set; }
            public override bool Equals(object? obj)
            {
            ServiceNotInContractViewResponse? item = obj as ServiceNotInContractViewResponse;

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
