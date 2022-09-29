using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        Task<ObjectModelResponse> Login(LoginRequest model);

    }

    public class AccountService : IAccountService
    {
        private readonly Database_UPODContext _context;
        private readonly IConfiguration _configuration;

        public AccountService(Database_UPODContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ObjectModelResponse> Login(LoginRequest model)
        {
            var user = await _context.Accounts.Where(a => a.Username!.Equals(model.username) && a.Password!.Equals(model.password)).FirstOrDefaultAsync();
            if (user == null)
            {
                return new ObjectModelResponse(user!)
                {
                    Type = "Account",
                    Message = "Invalid username or password!",
                    Status = 401,
                };
            }
            else
            {
                var account = new LoginResponse
                {
                    id = user.Id,
                    code = user.Code,
                    role_id = user.RoleId,
                    username = user.Username,
                    token = GenerateToken(user.Id, user.RoleId, user.Code!)
                };
                return new ObjectModelResponse(account!)
                {
                    Type = "Login",
                };
            }
        }
        public string GenerateToken(Guid accountId, Guid? roleId, string code)
        {
            var Claims = new List<Claim>
            {
                new Claim("RoleId", roleId.ToString()!),
                new Claim("AccountId", accountId.ToString()),
                new Claim("Code", code)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.Now.AddDays(int.Parse(_configuration["Jwt:DateExprise"])),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
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
