using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using IDeviceService = UPOD.SERVICES.Services.IDeviceService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/dervices")]
    public partial class DevicesController : ControllerBase
    {

        private readonly IDeviceService _device_sv;
        public DevicesController(IDeviceService device_sv)
        {
            _device_sv = device_sv;
        }

        [HttpGet]
        [Route("get_list_device")]
        public async Task<ResponseModel<DeviceResponse>> GetListDevices([FromQuery] PaginationRequest model)
        {
            return await _device_sv.GetListDevices(model);
        }

        [HttpGet]
        [Route("get_device_details_by_id")]
        public async Task<ObjectModelResponse> GetDetailsDevice(Guid id)
        {
            return await _device_sv.GetDetailsDevice(id);
        }
        [HttpPost]
        [Route("create_device")]
        public async Task<ObjectModelResponse> CreateDevice(DeviceRequest model)
        {
            return await _device_sv.CreateDevice(model);
        }
        [HttpPut]
        [Route("update_device_by_id")]
        public async Task<ObjectModelResponse> UpdateDervice(Guid id, DeviceUpdateRequest model)
        {
            return await _device_sv.UpdateDevice(id, model);
        }
        [HttpPut]
        [Route("disable_device_by_id")]
        public async Task<ObjectModelResponse> DisableDervice(Guid id)
        {
            return await _device_sv.DisableDevice(id);
        }

    }
}
