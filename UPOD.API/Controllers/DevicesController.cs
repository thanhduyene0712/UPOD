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

        private readonly IDeviceService _devicesv;
        public DevicesController(IDeviceService device_sv)
        {
            _devicesv = device_sv;
        }

        [HttpGet]
        [Route("get_list_device")]
        public async Task<ResponseModel<DeviceResponse>> GetListDevice([FromQuery] PaginationRequest model)
        {
            return await _devicesv.GetListDevice(model);
        }

        [HttpGet]
        [Route("get_detail_device_by_id")]
        public async Task<ResponseModel<DeviceResponse>> GetDetailDevice(Guid id)
        {
            return await _devicesv.GetDetailDevice(id);
        }
        [HttpPost]
        [Route("create")]
        public async Task<ResponseModel<DeviceResponse>> CreateDevice(DeviceRequest model)
        {
            return await _devicesv.CreateDevice(model);
        }
        [HttpPut]
        [Route("update_by_id")]
        public async Task<ResponseModel<DeviceResponse>> UpdateDervice(Guid id, DeviceUpdateRequest model)
        {
            return await _devicesv.UpdateDevice(id, model);
        }
        [HttpPut]
        [Route("disable_by_id")]
        public async Task<ResponseModel<DeviceResponse>> DisableDervice(Guid id)
        {
            return await _devicesv.DisableDevice(id);
        }


    }
}
