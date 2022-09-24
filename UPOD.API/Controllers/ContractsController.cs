//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Reso.Core.Utilities;
//using UPOD.REPOSITORIES.Models;
//using UPOD.REPOSITORIES.RequestModels;
//using UPOD.REPOSITORIES.ResponeModels;
//using IContractServiceService = UPOD.SERVICES.Services.IContractServiceService;

//namespace UPOD.API.Controllers
//{
//    [ApiController]
//    [Route("api/contracts")]
//    public partial class ContractsController : ControllerBase
//    {

//        private readonly IContractServiceService _contract_service_sv;
//        public ContractsController(IContractServiceService contract_service_sv)
//        {
//            _contract_service_sv = contract_service_sv;
//        }

//        [HttpGet]
//        [Route("get_all_contracts")]
//        public async Task<ResponseModel<ContractResponse>> GetAllContracs([FromQuery] PaginationRequest model)
//        {
//            return await _contract_service_sv.GetAll(model);
//        }

//        [HttpGet]
//        [Route("get_list_contracts")]
//        public async Task<ResponseModel<ContractListResponse>> GetListContract([FromQuery] PaginationRequest model)
//        {
//            return await _contract_service_sv.GetListContract(model);
//        }
//        [HttpGet]
//        [Route("get_contract_details_by_id")]
//        public async Task<ResponseModel<ContractDetailResponse>> GetDetailContract([FromQuery] Guid Id)
//        {
//            return await _contract_service_sv.GetDetailContract(Id);
//        }
//        [HttpPost]
//        [Route("create_contract")]
//        public async Task<ResponseModel<ContractResponse>> CreateContract(ContractRequest model)
//        {
//            return await _contract_service_sv.CreateContract(model);
//        }
//        [HttpPut]
//        [Route("disable_contract_by_id")]
//        public async Task<ResponseModel<ContractResponse>> DisableContract(Guid id)
//        {
//            return await _contract_service_sv.DisableContract(id);
//        }

//    }
//}
