using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using IGuidelineService = UPOD.SERVICES.Services.IGuidelineService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/guidelines")]
    public partial class GuidelinesController : ControllerBase
    {

        private readonly IGuidelineService _guideline_sv;
        public GuidelinesController(IGuidelineService guideline_sv)
        {
            _guideline_sv = guideline_sv;
        }

        [HttpGet]
        [Route("get_list_guideline")]
        public async Task<ResponseModel<GuidelineResponse>> GetListGuidelines([FromQuery] PaginationRequest model)
        {
            return await _guideline_sv.GetListGuidelines(model);
        }

        [HttpPost]
        [Route("create_guideline")]
        public async Task<ObjectModelResponse> CreateGuideline(GuidelineRequest model)
        {
            return await _guideline_sv.CreateGuideline(model);
        }
        [HttpPut]
        [Route("update_guideline_by_id")]
        public async Task<ObjectModelResponse> UpdateDervice(Guid id, GuidelineRequest model)
        {
            return await _guideline_sv.UpdateGuideline(id, model);
        }
        [HttpPut]
        [Route("disable_guideline_by_id")]
        public async Task<ObjectModelResponse> DisableDervice(Guid id)
        {
            return await _guideline_sv.DisableGuideline(id);
        }

    }
}
