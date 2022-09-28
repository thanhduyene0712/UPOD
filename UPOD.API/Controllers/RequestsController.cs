using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reso.Core.Utilities;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Enum;
using UPOD.SERVICES.Services;
using IRequestService = UPOD.SERVICES.Services.IRequestService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public partial class RequestsController : ControllerBase
    {

        private readonly IRequestService _request_sv;
        public RequestsController(IRequestService request_sv)
        {
            _request_sv = request_sv;
        }

        [HttpGet]
        [Route("get_list_requests")]
        public async Task<ResponseModel<RequestListResponse>> GetListRequests([FromQuery] PaginationRequest model, [FromQuery] FilterRequest status)
        {
            return await _request_sv.GetListRequests(model, status);
        }

        [HttpGet]
        [Route("get_request_details_by_id")]
        public async Task<ObjectModelResponse> GetDetailsRequest([FromQuery] PaginationRequest model, [FromQuery] Guid id)
        {
            return await _request_sv.GetDetailsRequest(model, id);
        }
        [HttpGet]
        [Route("get_technicians_by_id_request")]
        public async Task<ResponseModel<TechnicianRequestResponse>> GetTechnicianRequest([FromQuery] PaginationRequest model, [FromQuery] Guid id)
        {
            return await _request_sv.GetTechnicianRequest(model, id);
        }
        [HttpGet]
        [Route("get_devices_by_id_request")]
        public async Task<ResponseModel<DeviceResponse>> GetDeviceRequest([FromQuery] PaginationRequest model, Guid id)
        {
            return await _request_sv.GetDeviceRequest(model, id);
        }

        [HttpPost]
        [Route("create_request")]
        public async Task<ObjectModelResponse> CreateRequest(RequestRequest model)
        {
            return await _request_sv.CreateRequest(model);
        }

        [HttpPut]
        [Route("update_request_by_id")]
        public async Task<ObjectModelResponse> UpdateRequest([FromQuery] Guid id, RequestUpdateRequest model)
        {
            return await _request_sv.UpdateRequest(id, model);
        }
        [HttpPut]
        [Route("mapping_technician_to_request_by_id")]
        public async Task<ObjectModelResponse> MappingTechnicianRequest(Guid request_id, Guid technician_id)
        {
            return await _request_sv.MappingTechnicianRequest(request_id, technician_id);
        }
        [HttpPut]
        [Route("disable_request_by_id")]
        public async Task<ObjectModelResponse> DisableRequest([FromQuery] Guid id)
        {
            return await _request_sv.DisableRequest(id);
        }

    }
}
