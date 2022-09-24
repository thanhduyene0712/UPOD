using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{
    public interface IAccountService
    {
        Task<ResponseModel<AccountResponse>> GetAll(PaginationRequest model);
        Task<ObjectModelResponse> GetAccountDetails(Guid id);
        Task<ObjectModelResponse> UpdateAccount(Guid id, AccountUpdateRequest model);
        Task<ObjectModelResponse> CreateAccount(AccountRequest model);
        Task<ObjectModelResponse> DisableAccount(Guid id);


    }

    public class AccountService : IAccountService
    {
        private readonly Database_UPODContext _context;
        public AccountService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ObjectModelResponse> DisableAccount(Guid id)
        {
            var account = await _context.Accounts.Where(a => a.Id.Equals(id)).Include(a => a.Role).FirstOrDefaultAsync();
            account!.IsDelete = true;
            account.UpdateDate = DateTime.Now;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            var model = new AccountResponse
            {
                id = account.Id,
                code = account.Code,
                role = new RoleResponse
                {
                    id = account.Role!.Id,
                    code = account.Role.Code,
                    role_name = account.Role.RoleName,
                },
                username = account.Username,
                is_delete = account.IsDelete,
                create_date = account.CreateDate,
                update_date = account.UpdateDate,
            };
            return new ObjectModelResponse(model)
            {
                Type = "Account",
            };
        }

        public async Task<ResponseModel<AccountResponse>> GetAll(PaginationRequest model)
        {
            var accounts = await _context.Accounts.Where(a => a.IsDelete == false).Select(p => new AccountResponse
            {
                id = p.Id,
                code = p.Code,
                role = new RoleResponse
                {
                    id = p.Role!.Id,
                    code = p.Role.Code,
                    role_name = p.Role.RoleName,
                },
                username = p.Username,
                password = p.Password,
                is_delete = p.IsDelete,
                create_date = p.CreateDate,
                update_date = p.UpdateDate,

            }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AccountResponse>(accounts)
            {
                Total = accounts.Count,
                Type = "Accounts"
            };
        }
        public async Task<ObjectModelResponse> GetAccountDetails(Guid id)
        {
            var account = await _context.Accounts.Where(a => a.IsDelete == false && a.Id.Equals(id)).Select(p => new AccountResponse
            {
                id = p.Id,
                code = p.Code,
                role = new RoleResponse
                {
                    id = p.Role!.Id,
                    code = p.Role.Code,
                    role_name = p.Role.RoleName,
                },
                username = p.Username,
                password = p.Password,
                is_delete = p.IsDelete,
                create_date = p.CreateDate,
                update_date = p.UpdateDate,

            }).FirstOrDefaultAsync();
            return new ObjectModelResponse(account!)
            {
                Type = "Accounts"
            };
        }

        public async Task<ObjectModelResponse> CreateAccount(AccountRequest model)
        {
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("ACC", code_number + 1);
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Code = code,
                RoleId = model.role_id,
                Username = model.user_name,
                Password = model.password,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };
            var data = new AccountResponse();
            var message = "blank";
            var status = 500;
            var username = await _context.Accounts.Where(x => x.Username!.Equals(account.Username)).FirstOrDefaultAsync();
            if (username != null)
            {
                status = 400;
                message = "Username is already exists!";
            }
            else
            {

                message = "Successfully";
                status = 201;
                _context.Accounts.Add(account);
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    data = (new AccountResponse
                    {
                        id = account.Id,
                        code = account.Code,
                        role = new RoleResponse
                        {
                            id = _context.Roles.Where(a => a.Id.Equals(account.RoleId)).Select(a => a.Id).FirstOrDefault(),
                            code = _context.Roles.Where(a => a.Id.Equals(account.RoleId)).Select(a => a.Code).FirstOrDefault(),
                            role_name = _context.Roles.Where(a => a.Id.Equals(account.RoleId)).Select(a => a.RoleName).FirstOrDefault(),
                        },
                        username = account.Username,
                        is_delete = account.IsDelete,
                        create_date = account.CreateDate,
                        update_date = account.UpdateDate,
                    });
                }
            }
            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
                Type = "Account"
            };
        }
        public async Task<ObjectModelResponse> UpdateAccount(Guid id, AccountUpdateRequest model)
        {
            var account = await _context.Accounts.Where(a => a.Id.Equals(id)).Select(x => new Account
            {
                Id = id,
                Code = x.Code,
                RoleId = model.role_id,
                Username = x.Username,
                Password = model.password,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
            }).FirstOrDefaultAsync();
            _context.Accounts.Update(account!);
            var data = new AccountResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new AccountResponse
                {
                    id = account!.Id,
                    code = account.Code,
                    role = new RoleResponse
                    {
                        id = _context.Roles.Where(a => a.Id.Equals(account.RoleId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Roles.Where(a => a.Id.Equals(account.RoleId)).Select(a => a.Code).FirstOrDefault(),
                        role_name = _context.Roles.Where(a => a.Id.Equals(account.RoleId)).Select(a => a.RoleName).FirstOrDefault(),
                    },
                    username = account.Username,
                    password = account.Password,
                    is_delete = account.IsDelete,
                    create_date = account.CreateDate,
                    update_date = account.UpdateDate,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Account"
            };
        }
        private async Task<int> GetLastCode()
        {
            var account = await _context.Accounts.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(account!.Code!);
        }
    }

}
