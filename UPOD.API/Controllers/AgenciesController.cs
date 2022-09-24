using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.Repositories;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Services;
using IAgencyService = UPOD.SERVICES.Services.IAgencyService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/agencies")]
    public partial class AgenciesController : ControllerBase
    {

        private readonly IAgencyService _agency_sv;
        public AgenciesController(IAgencyService agency_sv)
        {
            _agency_sv = agency_sv;
        }

        [HttpGet]
        [Route("get_list_agencies")]
        public async Task<ResponseModel<AgencyResponse>> GetListAgencies([FromQuery] PaginationRequest model)
        {
            return await _agency_sv.GetListAgencies(model);
        }
        [HttpGet]
        [Route("get_agency_details")]
        public async Task<ObjectModelResponse> GetDetailsAgency(Guid id)
        {
            return await _agency_sv.GetDetailsAgency(id);
        }
        [HttpPost]
        [Route("create_agency")]
        public async Task<ObjectModelResponse> CreateAgency(AgencyRequest model)
        {
            return await _agency_sv.CreateAgency(model);
        }
        [HttpPut]
        [Route("update_agency_by_id")]
        public async Task<ObjectModelResponse> UpdateAgency(Guid id, AgencyUpdateRequest model)
        {
            return await _agency_sv.UpdateAgency(id, model);
        }
        [HttpPut]
        [Route("disable_agency_by_id")]
        public async Task<ObjectModelResponse> DisableAgency(Guid id)
        {
            return await _agency_sv.DisableAgency(id);
        }

    }
}
