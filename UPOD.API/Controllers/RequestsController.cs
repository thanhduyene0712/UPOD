using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reso.Core.Utilities;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Services;
using IRequestService = UPOD.SERVICES.Services.IRequestService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public partial class RequestsController : ControllerBase
    {

        private readonly IRequestService _RequestSv;
        public RequestsController(IRequestService RequestSv)
        {
            _RequestSv = RequestSv;
        }

        [HttpGet]
        [Route("get/list_request")]
        public async Task<ResponseModel<RequestResponse>> GetListRequest([FromQuery] PaginationRequest model)
        {
            return await _RequestSv.GetListRequest(model);
        }
        //[HttpGet]
        //[Route("get/list_agency_device")]
        //public async Task<ResponseModel<AgencyDeviceResponse>> GetListAgencyDevice([FromQuery] PaginationRequest model)
        //{
        //    return await _RequestSv.GetListAgencyDevice(model);
        //}

        [HttpGet]
        [Route("get/detail_request/id")]
        public async Task<ResponseModel<RequestDetailResponse>> GetDetailRequest([FromQuery] Guid id)
        {
            return await _RequestSv.GetDetailRequest(id);
        }
        [HttpGet]
        [Route("get/technician")]
        public async Task<ResponseModel<TechnicanResponse>> GetTechnicanRequest([FromQuery] PaginationRequest model,[FromQuery] Guid id)
        {
            return await _RequestSv.GetTechnicanRequest(model, id);
        }


        [HttpPost]
        [Route("create")]
        public async Task<ResponseModel<RequestCreateResponse>> CreateRequest(RequestRequest model)
        {
            return await _RequestSv.CreateRequest(model);
        }

        [HttpPut]
        [Route("update/id")]
        public async Task<ResponseModel<RequestCreateResponse>> UpdateRequest([FromQuery] Guid id, RequestUpdateRequest model)
        {
            return await _RequestSv.UpdateRequest(id, model);
        }
        [HttpPut]
        [Route("disable/id")]
        public async Task<ResponseModel<RequestDisableResponse>> DisableRequest([FromQuery] Guid id)
        {
            return await _RequestSv.DisableRequest(id);
        }

    }
}
