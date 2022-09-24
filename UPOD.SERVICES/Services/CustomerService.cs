using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Principal;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.REPOSITORIES.Services;
using UPOD.SERVICES.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UPOD.SERVICES.Services
{
    public interface ICustomerService
    {
        Task<ResponseModel<CustomerResponse>> GetAll(PaginationRequest model);
        Task<ObjectModelResponse> CreateCustomer(CustomerRequest model);
        Task<ObjectModelResponse> GetCustomerDetails(Guid id);
        Task<ObjectModelResponse> UpdateCustomer(Guid id, CustomerRequest model);
        Task<ObjectModelResponse> DisableCustomer(Guid id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly Database_UPODContext _context;
        public CustomerService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<CustomerResponse>> GetAll(PaginationRequest model)
        {
            var customers = await _context.Customers.Where(a => a.IsDelete == false).Select(a => new CustomerResponse
            {
                id = a.Id,
                code = a.Code,
                name = a.Name,
                account = new AccountViewResponse
                {
                    id = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Code).FirstOrDefault(),
                    role_name = _context.Roles.Where(x => x.Id.Equals(a.Account!.RoleId)).Select(x => x.RoleName).FirstOrDefault(),
                    username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
                    password = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Password).FirstOrDefault(),
                },

                description = a.Description,
                percent_for_technican_exp = a.PercentForTechnicianExp,
                percent_for_technican_rate = a.PercentForTechnicianRate,
                percent_for_technican_familiar_with_agency = a.PercentForTechnicianFamiliarWithAgency,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,


            }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<CustomerResponse>(customers)
            {
                Total = customers.Count,
                Type = "Customers"
            };
        }
        public async Task<ObjectModelResponse> GetCustomerDetails(Guid id)
        {
            var customer = await _context.Customers.Where(a => a.Id.Equals(id) && a.IsDelete == false).Include(x => x.Account).Select(a => new CustomerResponse
            {
                id = a.Id,
                code = a.Code,
                name = a.Name,
                account = new AccountViewResponse
                {
                    id = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Code).FirstOrDefault(),
                    role_name = _context.Roles.Where(x => x.Id.Equals(a.Account!.RoleId)).Select(x => x.RoleName).FirstOrDefault(),
                    username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
                    password = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Password).FirstOrDefault(),
                },
                description = a.Description,
                percent_for_technican_exp = a.PercentForTechnicianExp,
                percent_for_technican_rate = a.PercentForTechnicianRate,
                percent_for_technican_familiar_with_agency = a.PercentForTechnicianFamiliarWithAgency,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,


            }).ToListAsync();
            return new ObjectModelResponse(customer)
            {
                Type = "Customer"
            };
        }

        public async Task<ObjectModelResponse> CreateCustomer(CustomerRequest model)
        {
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("CU", code_number + 1);
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Code = code,
                Name = model.name,
                AccountId = model.account_id,
                Description = model.description,
                PercentForTechnicianExp = model.percent_for_technican_exp,
                PercentForTechnicianRate = model.percent_for_technican_rate,
                PercentForTechnicianFamiliarWithAgency = model.percent_for_technican_familiar_with_agency,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,

            };
            var message = "blank";
            var status = 500;
            var data = new CustomerResponse();
            var customer_name = await _context.Customers.Where(x => x.Name!.Equals(customer.Name)).FirstOrDefaultAsync();
            if (customer_name != null)
            {
                status = 400;
                message = "CustomerName is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Customers.AddAsync(customer);
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    var account = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).FirstOrDefault();
                    var role = _context.Roles.Where(x => x.Id.Equals(account!.RoleId)).FirstOrDefault();
                    data = new CustomerResponse
                    {
                        id = customer.Id,
                        code = customer.Code,
                        name = customer.Name,
                        account = new AccountViewResponse
                        {
                            id = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Id).FirstOrDefault(),
                            code = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Code).FirstOrDefault(),
                            role_name = role!.RoleName,
                            username = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Username).FirstOrDefault(),
                            password = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Password).FirstOrDefault(),
                        },
                        description = customer.Description,
                        percent_for_technican_exp = customer.PercentForTechnicianExp,
                        percent_for_technican_rate = customer.PercentForTechnicianRate,
                        percent_for_technican_familiar_with_agency = customer.PercentForTechnicianFamiliarWithAgency,
                        is_delete = customer.IsDelete,
                        create_date = customer.CreateDate,
                        update_date = customer.UpdateDate,
                    };
                }
            }

            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
                Type = "Customer"
            };
        }
        public async Task<ObjectModelResponse> DisableCustomer(Guid id)
        {
            var customer = await _context.Customers.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            customer!.IsDelete = true;
            customer.UpdateDate = DateTime.Now;
            _context.Customers.Update(customer);
            var rs = await _context.SaveChangesAsync();

            var data = new CustomerResponse();
            if (rs > 0)
            {
                var account = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).FirstOrDefault();
                var role = _context.Roles.Where(x => x.Id.Equals(account!.RoleId)).FirstOrDefault();
                data = new CustomerResponse
                {
                    id = customer.Id,
                    code = customer.Code,
                    name = customer.Name,
                    account = new AccountViewResponse
                    {
                        id = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Code).FirstOrDefault(),
                        role_name = role!.RoleName,
                        username = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Username).FirstOrDefault(),
                        password = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Password).FirstOrDefault(),
                    },
                    description = customer.Description,
                    percent_for_technican_exp = customer.PercentForTechnicianExp,
                    percent_for_technican_rate = customer.PercentForTechnicianRate,
                    percent_for_technican_familiar_with_agency = customer.PercentForTechnicianFamiliarWithAgency,
                    is_delete = customer.IsDelete,
                    create_date = customer.CreateDate,
                    update_date = customer.UpdateDate,

                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Customer"
            };
        }
        public async Task<ObjectModelResponse> UpdateCustomer(Guid id, CustomerRequest model)
        {
            var customer = await _context.Customers.Where(a => a.Id.Equals(id)).Select(x => new Customer
            {
                Id = id,
                Code = x.Code,
                Name = model.name,
                AccountId = model.account_id,
                Description = model.description,
                PercentForTechnicianExp = model.percent_for_technican_exp,
                PercentForTechnicianRate = model.percent_for_technican_rate,
                PercentForTechnicianFamiliarWithAgency = model.percent_for_technican_familiar_with_agency,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,

            }).FirstOrDefaultAsync();
            _context.Customers.Update(customer!);
            var rs = await _context.SaveChangesAsync();
            var data = new CustomerResponse();
            if (rs > 0)
            {
                var account = _context.Accounts.Where(x => x.Id.Equals(customer!.AccountId)).FirstOrDefault();
                var role = _context.Roles.Where(x => x.Id.Equals(account!.RoleId)).FirstOrDefault();
                data = new CustomerResponse
                {
                    id = customer!.Id,
                    code = customer.Code,
                    name = customer.Name,
                    account = new AccountViewResponse
                    {
                        id = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Code).FirstOrDefault(),
                        role_name = role!.RoleName,
                        username = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Username).FirstOrDefault(),
                        password = _context.Accounts.Where(x => x.Id.Equals(customer.AccountId)).Select(x => x.Password).FirstOrDefault(),
                    },
                    description = customer.Description,
                    percent_for_technican_exp = customer.PercentForTechnicianExp,
                    percent_for_technican_rate = customer.PercentForTechnicianRate,
                    percent_for_technican_familiar_with_agency = customer.PercentForTechnicianFamiliarWithAgency,
                    is_delete = customer.IsDelete,
                    create_date = customer.CreateDate,
                    update_date = customer.UpdateDate,

                };
            }
            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Customer"
            };
        }

        private async Task<int> GetLastCode()
        {
            var customer = await _context.Customers.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(customer!.Code!);
        }
    }
}
