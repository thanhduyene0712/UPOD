using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using IAreaService = UPOD.SERVICES.Services.IAreaService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/areas")]
    public partial class AreasController : ControllerBase
    {

        private readonly IAreaService _area_sv;
        public AreasController(IAreaService area_sv)
        {
            _area_sv = area_sv;
        }

        [HttpGet]
        [Route("get_list_area")]
        public async Task<ActionResult<ResponseModel<AreaResponse>>> GetListAreas([FromQuery] PaginationRequest model, [FromQuery] SearchRequest value)
        {
            try
            {
                return await _area_sv.GetListAreas(model, value);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_area_details_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> GetDetailsArea(Guid id)
        {
            try
            {
                return await _area_sv.GetDetailsArea(id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpGet]
        [Route("get_list_technicians_by_area_id")]
        public async Task<ActionResult<ResponseModel<TechnicianViewResponse>>> GetListTechniciansByAreaId([FromQuery]PaginationRequest model, Guid id)
        {
            try
            {
                return await _area_sv.GetListTechniciansByAreaId(model, id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [Route("create_area")]
        public async Task<ActionResult<ObjectModelResponse>> CreateArea(AreaRequest model)
        {
            try
            {
                return await _area_sv.CreateArea(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update_area_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateDervice(Guid id, AreaRequest model)
        {
            try
            {
                return await _area_sv.UpdateArea(id, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("disable_area_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> DisableDervice(Guid id)
        {
            try
            {
                return await _area_sv.DisableArea(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
