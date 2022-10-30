using StackExchange.Redis;
using UPOD.REPOSITORIES.ResponseModels;

namespace UPOD.REPOSITORIES.ResponseViewModel
{
    public class ListServiceResponse
    {
        public List<ServiceViewResponse> service { get; set; }
    }
    public class ServiceViewResponse
    {
        public Guid? id { get; set; }
        public string? code { get; set; }
        public string? service_name { get; set; }
        public string? description { get; set; }
        public int? frequency_maintain { get; set; }
       
    }

}
