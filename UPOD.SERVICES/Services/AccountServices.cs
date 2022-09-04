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
        Task<ResponseModel<AccountRespone>> GetAll(PaginationRequest model);
        Task<ResponseModel<AccountRespone>> SearchAccounts(PaginationRequest model, String value);
        Task<ResponseModel<AccountRespone>> UpdateAccount(Guid id, AccountRequest model);
        Task<ResponseModel<AccRegisterRespone>> CreateAccount(AccRegisterRequest model);



    }

    public class AccountService : IAccountService
    {
        private readonly Database_UPODContext _context;
        public AccountService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<AccountRespone>> GetAll(PaginationRequest model)
        {
            var accounts = await _context.Accounts.Select(p => new AccountRespone
            {
                Id = p.Id,
                RoleId = p.RoleId,
                Username = p.Username,
                IsDelete = p.IsDelete,
                CreateDate = p.CreateDate,
                UpdateDate = p.UpdateDate,

            }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AccountRespone>(accounts)
            {
                Total = accounts.Count,
                Type = "Accounts"
            };
        }
        public async Task<ResponseModel<AccountRespone>> SearchAccounts(PaginationRequest model, String value)
        {
            var accounts = await _context.Accounts.Where(p => (p.Username.Contains(value) 
            || p.Role.RoleName.Contains(value)
            || p.CreateDate.ToString().Contains(value))
            && p.IsDelete.ToString().Equals("false")).Select(p => new AccountRespone
            {
                Id = p.Id,
                RoleId = p.RoleId,
                Username = p.Username,
                IsDelete = p.IsDelete,
                CreateDate = p.CreateDate,
                UpdateDate = p.UpdateDate,
            }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AccountRespone>(accounts)
            {
                Total = accounts.Count,
                Type = "Accounts"
            };
        }
        public async Task<ResponseModel<AccRegisterRespone>> CreateAccount(AccRegisterRequest model)
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                RoleId = await _context.Roles.Where(x => x.RoleName.Equals(model.RoleName)).Select(x => x.Id).FirstOrDefaultAsync(),
                Username = model.Username,
                Password = model.Password,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = null,

            };
            var list = new List<AccRegisterRespone>();
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
                list.Add(new AccRegisterRespone
                {
                    Id = account.Id,
                    RoleName = await _context.Roles.Where(x => x.Id.Equals(account.RoleId)).Select(x => x.RoleName).FirstOrDefaultAsync(),
                    Username = account.Username,
                    CreateDate = account.CreateDate,
                });
            }
            return new ResponseModel<AccRegisterRespone>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Account"
            };
        }
        public async Task<ResponseModel<AccountRespone>> UpdateAccount(Guid id, AccountRequest model)
        {
            var account = await _context.Accounts.Where(x => x.Id.Equals(id)).Select(x => new Account
            {
                Id = id,
                RoleId = _context.Roles.Where(x => x.RoleName.Equals(model.RoleName)).Select(x => x.Id).FirstOrDefault(),
                Username = x.Username,
                Password = model.Password,
                IsDelete = model.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
            }).FirstOrDefaultAsync();
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            var list = new List<AccountRespone>();
            list.Add(new AccountRespone
            {
                Id = account.Id,
                RoleId = account.RoleId,
                Username = account.Username,
                IsDelete = account.IsDelete,
                CreateDate = account.CreateDate,
                UpdateDate = account.UpdateDate,
            });
            return new ResponseModel<AccountRespone>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Account"
            };
        }

    }
}
