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
        public async Task<ActionResult<ResponseModel<ServiceResponse>>> GetAllServices([FromQuery] PaginationRequest model)
        {
            try
            {
                return await _service_sv.GetAll(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_service_details")]
        public async Task<ActionResult<ObjectModelResponse>> GetServiceDetails(Guid id)
        {
            try
            {
                return await _service_sv.GetServiceDetails(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create_service")]
        public async Task<ActionResult<ObjectModelResponse>> CreateService(ServiceRequest model)
        {
            try
            {
                return await _service_sv.CreateService(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update_service_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateService([FromQuery] Guid id, ServiceRequest model)
        {
            try
            {
                return Ok(await _service_sv.UpdateService(id, model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("disable_service_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> DisableService([FromQuery] Guid id)
        {
            try
            {
                return Ok(await _service_sv.DisableService(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
