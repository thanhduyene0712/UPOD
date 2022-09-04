using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reso.Core.Utilities;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.Services;
using UPOD.SERVICES.Services;
using IAgencyService = UPOD.SERVICES.Services.IAgencyService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/Agencies")]
    public partial class AgencysController : ControllerBase
    {

        private readonly IAgencyService _AgencySv;
        public AgencysController(IAgencyService AgencySv)
        {
            _AgencySv = AgencySv;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ResponseModel<AgencyRespone>> GetAllAgencies([FromQuery] PaginationRequest model)
        {
            return await _AgencySv.GetAll(model);
        }

        [HttpGet]
        [Route("Search")]
        public async Task<ResponseModel<AgencyRespone>> SearchAgencies([FromQuery] PaginationRequest model, String value)
        {
            return await _AgencySv.SearchAgencies(model, value);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ResponseModel<AgencyRespone>> CreateAgency([FromQuery] AgencyRequest model)
        {
            return await _AgencySv.CreateAgency(model);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<ResponseModel<AgencyRespone>> UpdateAgency(Guid id, [FromQuery] AgencyRequest model)
        {
            return await _AgencySv.UpdateAgency(id, model);
        }
        [HttpPut]
        [Route("Deisable")]
        public async Task<ResponseModel<AgencyRespone>> DisableAgency(Guid id, [FromQuery] AgencyDisableRequest model)
        {
            return await _AgencySv.DisableAgency(id, model);
        }


    }
}
