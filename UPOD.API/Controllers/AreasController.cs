using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
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
        public async Task<ResponseModel<AreaResponse>> GetListAreas([FromQuery] PaginationRequest model)
        {
            return await _area_sv.GetListAreas(model);
        }

        [HttpPost]
        [Route("create_area")]
        public async Task<ObjectModelResponse> CreateArea(AreaRequest model)
        {
            return await _area_sv.CreateArea(model);
        }
        [HttpPut]
        [Route("update_area_by_id")]
        public async Task<ObjectModelResponse> UpdateDervice(Guid id, AreaRequest model)
        {
            return await _area_sv.UpdateArea(id, model);
        }
        [HttpPut]
        [Route("disable_area_by_id")]
        public async Task<ObjectModelResponse> DisableDervice(Guid id)
        {
            return await _area_sv.DisableArea(id);
        }

    }
}
