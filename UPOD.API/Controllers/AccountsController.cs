using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reso.Core.Utilities;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Services;
using IAccountService = UPOD.SERVICES.Services.IAccountService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public partial class AccountsController : ControllerBase
    {

        private readonly IAccountService _account_sv;
        public AccountsController(IAccountService account_sv)
        {
            _account_sv = account_sv;
        }

        [HttpGet]
        [Route("get_all_accounts")]
        public async Task<ResponseModel<AccountResponse>> GetAllAccounts([FromQuery] PaginationRequest model)
        {
            return await _account_sv.GetAll(model);
        }

        [HttpGet]
        [Route("search_accounts_by_id")]
        public async Task<ObjectModelResponse> GetAccountDetails(Guid id)
        {
            return await _account_sv.GetAccountDetails(id);
        }
        [HttpPost]
        [Route("create_account")]
        public async Task<ObjectModelResponse> CreateAccount(AccountRequest model)
        {
            return await _account_sv.CreateAccount(model);
        }
        [HttpPut]
        [Route("update_account_by_id")]
        public async Task<ObjectModelResponse> UpdateAccount(Guid id, AccountUpdateRequest model)
        {
            return await _account_sv.UpdateAccount(id, model);
        }

        [HttpPut]
        [Route("disable_account_by_id")]
        public async Task<ObjectModelResponse> DisableAccount(Guid id)
        {
            return await _account_sv.DisableAccount(id);
        }

    }
}
