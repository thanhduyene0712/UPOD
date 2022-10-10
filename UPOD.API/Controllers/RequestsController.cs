﻿using AutoMapper;
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
        public async Task<ActionResult<ResponseModel<RequestListResponse>>> GetListRequests([FromQuery] PaginationRequest model, [FromQuery] FilterRequest status)
        {
            try
            {
                return await _request_sv.GetListRequests(model, status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get_request_details_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> GetDetailsRequest([FromQuery] Guid id)
        {
            try
            {
                return await _request_sv.GetDetailsRequest(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_technicians_by_id_request")]
        public async Task<ActionResult<ResponseModel<TechnicianRequestResponse>>> GetTechnicianRequest([FromQuery] PaginationRequest model, [FromQuery] Guid id)
        {
            try
            {
                return await _request_sv.GetTechnicianRequest(model, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_devices_by_id_request")]
        public async Task<ActionResult<ResponseModel<DeviceResponse>>> GetDeviceRequest([FromQuery] PaginationRequest model, Guid id)
        {
            try
            {
                return await _request_sv.GetDeviceRequest(model, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create_request")]
        public async Task<ActionResult<ObjectModelResponse>> CreateRequest(RequestRequest model)
        {

            try
            {
                return await _request_sv.CreateRequest(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("create_request_by_admin")]
        public async Task<ActionResult<ObjectModelResponse>> CreateRequestByAdmin(RequestRequest model)
        {

            try
            {
                return await _request_sv.CreateRequestByAdmin(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut]
        [Route("update_request_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateRequest([FromQuery] Guid id, RequestUpdateRequest model)
        {
            try
            {
                return await _request_sv.UpdateRequest(id, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("mapping_technician_to_request_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> MappingTechnicianRequest(Guid request_id, Guid technician_id)
        {
            try
            {
                return await _request_sv.MappingTechnicianRequest(request_id, technician_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("disable_request_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> DisableRequest([FromQuery] Guid id)
        {
            try
            {
                return await _request_sv.DisableRequest(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
