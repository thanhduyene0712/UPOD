using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{
    public interface IAdminService
    {
        Task<ResponseModel<AdminResponse>> GetListAdmins(PaginationRequest model, FilterRequest value);
        Task<ObjectModelResponse> CreateAdmin(AdminRequest model);
        Task<ObjectModelResponse> UpdateAdmin(Guid id, AdminRequest model);
        Task<ObjectModelResponse> DisableAdmin(Guid id);
    }

    public class AdminServices : IAdminService
    {
        private readonly Database_UPODContext _context;
        public AdminServices(Database_UPODContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<AdminResponse>> GetListAdmins(PaginationRequest model, FilterRequest value)
        {
            var admins = new List<AdminResponse>();
            if (value.search != null)
            {
                admins = await _context.Admins.Where(a => a.IsDelete == false
                 && (a.Name!.Contains(value.search)
                 || a.Mail!.Contains(value.search)
                 || a.Telephone!.Contains(value.search)
                 || a.Code!.Contains(value.search))).Select(a => new AdminResponse
                 {
                     id = a.Id,
                     code = a.Code,
                     name = a.Name,
                     account_id = a.AccountId,
                     create_date = a.CreateDate,
                     is_delete = a.IsDelete,
                     update_date = a.UpdateDate,
                     mail = a.Mail,
                     telephone = a.Telephone,
                 }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                admins = await _context.Admins.Where(a => a.IsDelete == false).Select(a => new AdminResponse
                {
                    id = a.Id,
                    code = a.Code,
                    name = a.Name,
                    account_id = a.AccountId,
                    create_date = a.CreateDate,
                    is_delete = a.IsDelete,
                    update_date = a.UpdateDate,
                    mail = a.Mail,
                    telephone = a.Telephone,
                }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<AdminResponse>(admins)
            {
                Total = admins.Count,
                Type = "Admins"
            };
        }

        public async Task<ObjectModelResponse> CreateAdmin(AdminRequest model)
        {
            var admin_id = Guid.NewGuid();
            while (true)
            {
                var admin_dup = await _context.Admins.Where(x => x.Id.Equals(admin_id)).FirstOrDefaultAsync();
                if (admin_dup == null)
                {
                    break;
                }
                else
                {
                    admin_id = Guid.NewGuid();
                }
            }
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("AD", code_number + 1);
            var admin = new Admin
            {
                Id = admin_id,
                Code = code,
                Name = model.name,
                Mail = model.mail,
                Telephone = model.telephone,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                AccountId = model.account_id,
            };

            var data = new AdminResponse();
            var message = "blank";
            var status = 500;
            var id = await _context.Admins.Where(a => a.AccountId.Equals(admin.AccountId)).FirstOrDefaultAsync();

            if (id != null)
            {
                message = "Account has been assigned to another admin exists!";
                status = 400;
            }
            else
            {
                status = 201;
                message = "Successfully";
                await _context.Admins.AddAsync(admin);
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    data = new AdminResponse
                    {
                        id = admin.Id,
                        code = admin.Code,
                        name = admin.Name,
                        account_id = admin.AccountId,
                        create_date = admin.CreateDate,
                        is_delete = admin.IsDelete,
                        update_date = admin.UpdateDate,
                        mail = admin.Mail,
                        telephone = admin.Telephone,
                    };
                }
            }

            return new ObjectModelResponse(data)
            {
                Status = status,
                Message = message,
                Type = "Admin"
            };
        }


        public async Task<ObjectModelResponse> UpdateAdmin(Guid id, AdminRequest model)
        {
            var admin = await _context.Admins.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new Admin
            {
                Id = id,
                Code = a.Code,
                Name = model.name,
                Mail = model.mail,
                Telephone = model.telephone,
                IsDelete = a.IsDelete,
                CreateDate = a.CreateDate,
                UpdateDate = DateTime.Now,
                AccountId = model.account_id,
            }).FirstOrDefaultAsync();
            var data = new AdminResponse();
            _context.Admins.Update(admin!);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new AdminResponse
                {
                    id = admin!.Id,
                    code = admin.Code,
                    name = admin.Name,
                    account_id = admin.AccountId,
                    create_date = admin.CreateDate,
                    is_delete = admin.IsDelete,
                    update_date = admin.UpdateDate,
                    mail = admin.Mail,
                    telephone = admin.Telephone,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Admin"
            };
        }

        public async Task<ObjectModelResponse> DisableAdmin(Guid id)
        {
            var admin = await _context.Admins.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            admin!.IsDelete = true;
            admin.UpdateDate = DateTime.Now;
            var data = new AdminResponse();
            _context.Admins.Update(admin!);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new AdminResponse
                {
                    id = admin.Id,
                    code = admin.Code,
                    name = admin.Name,
                    account_id = admin.AccountId,
                    create_date = admin.CreateDate,
                    is_delete = admin.IsDelete,
                    update_date = admin.UpdateDate,
                    mail = admin.Mail,
                    telephone = admin.Telephone,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Admin"
            };
        }
        private async Task<int> GetLastCode()
        {
            var admin = await _context.Admins.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(admin!.Code!);
        }
    }
}
