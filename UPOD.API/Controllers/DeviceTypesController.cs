using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Services;

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
        public async Task<ActionResult<ResponseModel<DeviceTypeResponse>>> GetListDeviceTypes([FromQuery] PaginationRequest model)
        {
            try
            {
                return await _device_type_sv.GetListDeviceTypes(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("create_device_type")]
        public async Task<ActionResult<ObjectModelResponse>> CreateDeviceType(DeviceTypeRequest model)
        {
            try
            {
                return await _device_type_sv.CreateDeviceType(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update_device_type_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateDerviceType(Guid id, DeviceTypeRequest model)
        {
            try
            {
                return await _device_type_sv.UpdateDeviceType(id, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("disable_device_type_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> DisableDerviceType(Guid id)
        {
            try
            {
                return await _device_type_sv.DisableDeviceType(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
