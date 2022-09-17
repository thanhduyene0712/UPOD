using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Reso.Core.BaseConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.Repositories;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;

namespace UPOD.SERVICES.Services
{
    public interface IAccountService
    {
        Task<ResponseModel<AccountResponse>> GetAll(PaginationRequest model);
        Task<ResponseModel<AccountResponse>> SearchAccounts(PaginationRequest model, String value);
        Task<ResponseModel<AccountResponse>> UpdateAccount(Guid id, AccountRequest model);
        Task<ResponseModel<AccRegisterResponse>> CreateAccount(AccRegisterRequest model);



    }

    public class AccountService : IAccountService
    {
        private readonly Database_UPODContext _context;
        public AccountService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<AccountResponse>> GetAll(PaginationRequest model)
        {
            var accounts = await _context.Accounts.Select(p => new AccountResponse
            {
                id = p.Id,
                role_id = p.RoleId,
                username = p.Username,
                is_delete = p.IsDelete,
                create_date = p.CreateDate,
                update_date = p.UpdateDate,

            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AccountResponse>(accounts)
            {
                Total = accounts.Count,
                Type = "Accounts"
            };
        }
        public async Task<ResponseModel<AccountResponse>> SearchAccounts(PaginationRequest model, String value)
        {
            var accounts = await _context.Accounts.Where(p => (p.Username.Contains(value) 
            || p.Role.RoleName.Contains(value)
            || p.CreateDate.ToString().Contains(value))
            && p.IsDelete.ToString().Equals("false")).Select(p => new AccountResponse
            {
                id = p.Id,
                role_id = p.RoleId,
                username = p.Username,
                is_delete = p.IsDelete,
                create_date = p.CreateDate,
                update_date = p.UpdateDate,
            }).OrderBy(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AccountResponse>(accounts)
            {
                Total = accounts.Count,
                Type = "Accounts"
            };
        }
        public async Task<ResponseModel<AccRegisterResponse>> CreateAccount(AccRegisterRequest model)
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                RoleId = await _context.Roles.Where(x => x.RoleName.Equals(model.role_name)).Select(x => x.Id).FirstOrDefaultAsync(),
                Username = model.user_name,
                Password = model.password,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = null,

            };
            var list = new List<AccRegisterResponse>();
            var message = "blank";
            var status = 500;
            var username = await _context.Accounts.Where(x => x.Username.Equals(account.Username)).FirstOrDefaultAsync();
            if (username != null)
            {
                status = 400;
                message = "Username is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                list.Add(new AccRegisterResponse
                {
                    id = account.Id,
                    role_name = await _context.Roles.Where(x => x.Id.Equals(account.RoleId)).Select(x => x.RoleName).FirstOrDefaultAsync(),
                    user_name = account.Username,
                    create_date = account.CreateDate,
                });
            }
            return new ResponseModel<AccRegisterResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Account"
            };
        }
        public async Task<ResponseModel<AccountResponse>> UpdateAccount(Guid id, AccountRequest model)
        {
            var account = await _context.Accounts.Where(a => a.Id.Equals(id)).Select(x => new Account
            {
                Id = id,
                RoleId = _context.Roles.Where(x => x.RoleName.Equals(model.role_name)).Select(x => x.Id).FirstOrDefault(),
                Username = x.Username,
                Password = model.password,
                IsDelete = model.is_delete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
            }).FirstOrDefaultAsync();
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            var list = new List<AccountResponse>();
            list.Add(new AccountResponse
            {
                id = account.Id,
                role_id = account.RoleId,
                username = account.Username,
                is_delete = account.IsDelete,
                create_date = account.CreateDate,
                update_date = account.UpdateDate,
            });
            return new ResponseModel<AccountResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Account"
            };
        }

    }
}
