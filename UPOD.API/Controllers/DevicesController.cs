using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using IDeviceService = UPOD.SERVICES.Services.IDeviceService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public partial class DevicesController : ControllerBase
    {

        private readonly IDeviceService _device_sv;
        public DevicesController(IDeviceService device_sv)
        {
            _device_sv = device_sv;
        }

        [HttpGet]
        [Route("get_list_devices")]
        public async Task<ActionResult<ResponseModel<DeviceResponse>>> GetListDevices([FromQuery] PaginationRequest model,[FromQuery] SearchRequest value)
        {
            try
            {
                return await _device_sv.GetListDevices(model, value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_list_devices_by_device_type_id")]
        public async Task<ActionResult<ResponseModel<DeviceResponse>>> GetListDevicesByDeviceType([FromQuery] PaginationRequest model, [FromQuery] SearchRequest value, Guid id)
        {
            try
            {
                return await _device_sv.GetListDevicesByDeviceType(model, value, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        [Route("get_list_devices_by_agency_id")]
        public async Task<ActionResult<ResponseModel<DeviceResponse>>> GetListDevicesAgency([FromQuery]PaginationRequest model, Guid id, [FromQuery] SearchRequest value)

        {
            try
            {
                return await _device_sv.GetListDevicesAgency(model, id, value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_device_details_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> GetDetailsDevice(Guid id)
        {
            try
            {
                return await _device_sv.GetDetailsDevice(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("create_device")]
        public async Task<ActionResult<ObjectModelResponse>> CreateDevice(DeviceRequest model)
        {
            try
            {
                return await _device_sv.CreateDevice(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update_device_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateDervice(Guid id, DeviceUpdateRequest model)
        {
            try
            {
                return await _device_sv.UpdateDevice(id, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("disable_device_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> DisableDervice(Guid id)
        {
            try
            {
                return await _device_sv.DisableDevice(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
