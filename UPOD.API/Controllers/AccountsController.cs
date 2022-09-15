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

        private readonly IAccountService _accountSv;
        public AccountsController(IAccountService accountSv)
        {
            _accountSv = accountSv;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ResponseModel<AccountResponse>> GetAllAccounts([FromQuery] PaginationRequest model)
        {
            return await _accountSv.GetAll(model);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ResponseModel<AccountResponse>> SearchAccounts([FromQuery] PaginationRequest model, String value)
        {
            return await _accountSv.SearchAccounts(model, value);
        }
        [HttpPost]
        [Route("create")]
        public async Task<ResponseModel<AccRegisterResponse>> CreateAccount(AccRegisterRequest model)
        {
            return await _accountSv.CreateAccount(model);
        }
        [HttpPut]
        [Route("update")]
        public async Task<ResponseModel<AccountResponse>> UpdateAccount(Guid id, [FromQuery] AccountRequest model)
        {
            return await _accountSv.UpdateAccount(id, model);
        }
       


    }
}
