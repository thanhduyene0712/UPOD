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
using IContractServiceService = UPOD.SERVICES.Services.IContractServiceService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/Contracts")]
    public partial class ContractsController : ControllerBase
    {

        private readonly IContractServiceService _contractServiceSv;
        public ContractsController(IContractServiceService contractServiceSv)
        {
            _contractServiceSv = contractServiceSv;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ResponseModel<ContractRespone>> GetAllContracs([FromQuery] PaginationRequest model)
        {
            return await _contractServiceSv.GetAll(model);
        }

        //[HttpGet]
        //[Route("Search")]
        //public async Task<ResponseModel<ContractRespone>> SearchAgencies([FromQuery] PaginationRequest model, String value)
        //{
        //    return await _ContractSv.SearchAgencies(model, value);
        //}
        [HttpPost]
        [Route("Create")]
        public async Task<ResponseModel<ContractRespone>> CreateContract([FromQuery] ContractRequest model)
        {
            return await _contractServiceSv.CreateContract(model);
        }
        //[HttpPut]
        //[Route("Update")]
        //public async Task<ResponseModel<ContractRespone>> UpdateContract(Guid id, [FromQuery] ContractRequest model)
        //{
        //    return await _ContractSv.UpdateContract(id, model);
        //}
        //[HttpPut]
        //[Route("Deisable")]
        //public async Task<ResponseModel<ContractRespone>> DisableContract(Guid id, [FromQuery] ContractDisableRequest model)
        //{
        //    return await _ContractSv.DisableContract(id, model);
        //}


    }
}
