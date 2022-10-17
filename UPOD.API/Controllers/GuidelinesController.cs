using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
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
        [Route("get_list_guidelines")]
        public async Task<ActionResult<ResponseModel<GuidelineResponse>>> GetListGuidelines([FromQuery] PaginationRequest model)
        {
            try
            {
                return await _guideline_sv.GetListGuidelines(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create_guideline")]
        public async Task<ActionResult<ObjectModelResponse>> CreateGuideline(GuidelineRequest model)
        {
            try
            {
                return await _guideline_sv.CreateGuideline(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update_guideline_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateDervice(Guid id, GuidelineRequest model)
        {
            try
            {
                return await _guideline_sv.UpdateGuideline(id, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("disable_guideline_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> DisableDervice(Guid id)
        {
            try
            {
                return await _guideline_sv.DisableGuideline(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
