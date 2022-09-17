using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reso.Core.Utilities;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using IContractServiceService = UPOD.SERVICES.Services.IContractServiceService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    public partial class ContractsController : ControllerBase
    {

        private readonly IContractServiceService _contractServiceSv;
        public ContractsController(IContractServiceService contractServiceSv)
        {
            _contractServiceSv = contractServiceSv;
        }

        [HttpGet]
        [Route("get_all")]
        public async Task<ResponseModel<ContractResponse>> GetAllContracs([FromQuery] PaginationRequest model)
        {
            return await _contractServiceSv.GetAll(model);
        }

        [HttpGet]
        [Route("get_list_contract")]
        public async Task<ResponseModel<ContractListResponse>> GetListContract([FromQuery] PaginationRequest model)
        {
            return await _contractServiceSv.GetListContract(model);
        }
        [HttpGet]
        [Route("get_detail_contract/id")]
        public async Task<ResponseModel<ContractDetailResponse>> GetDetailContract([FromQuery] Guid Id)
        {
            return await _contractServiceSv.GetDetailContract(Id);
        }
        [HttpPost]
        [Route("create")]
        public async Task<ResponseModel<ContractResponse>> CreateContract(/*[FromQuery]*/ ContractRequest model)
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
