using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using IServiceService = UPOD.SERVICES.Services.IServiceService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/Services")]
    public partial class ServicesController : ControllerBase
    {

        private readonly IServiceService _serviceSv;
        public ServicesController(IServiceService serviceSv)
        {
            _serviceSv = serviceSv;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ResponseModel<ServiceResponse>> GetAllService([FromQuery] PaginationRequest model)
        {
            return await _serviceSv.GetAll(model);
        }

        //[HttpGet]
        //[Route("Search")]
        //public async Task<ResponseModel<ServiceRespone>> SearchAgencies([FromQuery] PaginationRequest model, String value)
        //{
        //    return await _ServiceSv.SearchAgencies(model, value);
        //}
        [HttpPost]
        [Route("Create")]
        public async Task<ResponseModel<ServiceResponse>> CreateService([FromQuery] ServiceRequest model)
        {
            return await _serviceSv.CreateService(model);
        }
        //[HttpPut]
        //[Route("Update")]
        //public async Task<ResponseModel<ServiceRespone>> UpdateService(Guid id, [FromQuery] ServiceRequest model)
        //{
        //    return await _ServiceSv.UpdateService(id, model);
        //}
        //[HttpPut]
        //[Route("Deisable")]
        //public async Task<ResponseModel<ServiceRespone>> DisableService(Guid id, [FromQuery] ServiceDisableRequest model)
        //{
        //    return await _ServiceSv.DisableService(id, model);
        //}


    }
}
