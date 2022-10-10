using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserAccessor _userAccessor;

        public AccountsController(IAccountService account_sv, IUserAccessor userAccessor)
        {
            _account_sv = account_sv;
            _userAccessor = userAccessor;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult<ObjectModelResponse>> Login(LoginRequest model)
        {
            try
            {
                return await _account_sv.Login(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_all_accounts")]
        public async Task<ActionResult<ResponseModel<AccountResponse>>> GetAllAccounts([FromQuery] PaginationRequest model)
        {
            //var accountRole = _userAccessor.GetRoleId();
            //if (accountRole != Guid.Parse("dd3cb3b4-84fe-432e-bb06-2d8aecaa640d"))
            //{
            //    return BadRequest("Don't have permission!");
            //}
            try
            {
                return await _account_sv.GetAll(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("search_accounts_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> GetAccountDetails(Guid id)
        {
            try
            {
                return await _account_sv.GetAccountDetails(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("create_account")]
        public async Task<ActionResult<ObjectModelResponse>> CreateAccount(AccountRequest model)
        {
            try
            {
                return await _account_sv.CreateAccount(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update_account_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateAccount(Guid id, AccountUpdateRequest model)
        {
            try
            {
                return await _account_sv.UpdateAccount(id, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("disable_account_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> DisableAccount(Guid id)
        {
            try
            {
                return await _account_sv.DisableAccount(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
