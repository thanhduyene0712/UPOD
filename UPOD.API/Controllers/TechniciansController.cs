using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.Repositories;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Services;
using ITechnicianService = UPOD.SERVICES.Services.ITechnicianService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/technicians")]
    public partial class TechnicianController : ControllerBase
    {

        private readonly ITechnicianService _technician_sv;
        public TechnicianController(ITechnicianService technician_sv)
        {
            _technician_sv = technician_sv;
        }

        [HttpGet]
        [Route("get_list_technicians")]
        public async Task<ResponseModel<TechnicianResponse>> GetListTechnicians([FromQuery] PaginationRequest model)
        {
            return await _technician_sv.GetListTechnicians(model);
        }
        [HttpGet]
        [Route("get_technician_details")]
        public async Task<ResponseModel<TechnicianResponse>> GetDetailTechnician(Guid id)
        {
            return await _technician_sv.GetDetailTechnician(id);
        }
        [HttpPost]
        [Route("create_technician")]
        public async Task<ResponseModel<TechnicianResponse>> CreateTechnician(TechnicianRequest model)
        {
            return await _technician_sv.CreateTechnician(model);
        }
        [HttpPut]
        [Route("update_technician_by_id")]
        public async Task<ResponseModel<TechnicianResponse>> UpdateTechnician(Guid id, TechnicianRequest model)
        {
            return await _technician_sv.UpdateTechnician(id, model);
        }
        [HttpPut]
        [Route("disable_technician_by_id")]
        public async Task<ResponseModel<TechnicianResponse>> DisableTechnician(Guid id)
        {
            return await _technician_sv.DisableTechnician(id);
        }

    }
}
