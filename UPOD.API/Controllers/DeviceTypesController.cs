using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.Repositories;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Services;
using IDeviceService = UPOD.SERVICES.Services.IDeviceService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/dervice_types")]
    public partial class DeviceTypesController : ControllerBase
    {

        private readonly IDeviceTypeService _device_type_sv;
        public DeviceTypesController(IDeviceTypeService device_type_sv)
        {
            _device_type_sv = device_type_sv;
        }

        [HttpGet]
        [Route("get_list_device_type")]
        public async Task<ResponseModel<DeviceTypeResponse>> GetListDeviceType([FromQuery] PaginationRequest model)
        {
            return await _device_type_sv.GetListDeviceType(model);
        }
        [HttpPost]
        [Route("create_device_type")]
        public async Task<ResponseModel<DeviceTypeResponse>> CreateDeviceType(DeviceTypeRequest model)
        {
            return await _device_type_sv.CreateDeviceType(model);
        }
        [HttpPut]
        [Route("update_device_type_by_id")]
        public async Task<ResponseModel<DeviceTypeResponse>> UpdateDerviceType(Guid id, DeviceTypeRequest model)
        {
            return await _device_type_sv.UpdateDeviceType(id, model);
        }
        [HttpPut]
        [Route("disable_device_type_by_id")]
        public async Task<ResponseModel<DeviceTypeResponse>> DisableDerviceType(Guid id)
        {
            return await _device_type_sv.DisableDeviceType(id);
        }

    }
}
