using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using IServiceService = UPOD.SERVICES.Services.IServiceService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/services")]
    public partial class ServicesController : ControllerBase
    {

        private readonly IServiceService _service_sv;
        public ServicesController(IServiceService service_sv)
        {
            _service_sv = service_sv;
        }

        [HttpGet]
        [Route("get_all_services")]
        public async Task<ResponseModel<ServiceResponse>> GetAllServices([FromQuery] PaginationRequest model)
        {
            return await _service_sv.GetAll(model);
        }
        [HttpGet]
        [Route("get_service_details")]
        public async Task<ObjectModelResponse> GetServiceDetails(Guid id)
        {
            return await _service_sv.GetServiceDetails(id);
        }

        [HttpPost]
        [Route("create_service")]
        public async Task<ObjectModelResponse> CreateService(ServiceRequest model)
        {
            return await _service_sv.CreateService(model);
        }
        [HttpPut]
        [Route("update_service_by_id")]
        public async Task<ObjectModelResponse> UpdateService([FromQuery] Guid id, ServiceRequest model)
        {
            return await _service_sv.UpdateService(id, model);
        }
        [HttpPut]
        [Route("disable_service_by_id")]
        public async Task<ObjectModelResponse> DisableService([FromQuery] Guid id)
        {
            return await _service_sv.DisableService(id);
        }



    }
}
