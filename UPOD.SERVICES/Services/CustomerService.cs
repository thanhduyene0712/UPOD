using Microsoft.EntityFrameworkCore;
using System.Data;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{
    public interface ICustomerService
    {
        Task<ResponseModel<CustomerResponse>> GetAll(PaginationRequest model);
        Task<ObjectModelResponse> CreateCustomer(CustomerRequest model);
        Task<ObjectModelResponse> GetCustomerDetails(Guid id);
        Task<ObjectModelResponse> UpdateCustomer(Guid id, CustomerRequest model);
        Task<ObjectModelResponse> DisableCustomer(Guid id);
        Task<ObjectModelResponse> GetServiceByCustomerId(Guid id);
        Task<ResponseModel<AgencyOfCustomerResponse>> GetAgenciesByCustomerId(Guid id);
        Task<ResponseModel<RequestListResponse>> GetListRequestsByCustomerId(PaginationRequest model, FilterRequest status, Guid id);

    }

    public class CustomerService : ICustomerService
    {
        private readonly Database_UPODContext _context;
        public CustomerService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<RequestListResponse>> GetListRequestsByCustomerId(PaginationRequest model, FilterRequest status, Guid id)
        {

            var requests = new List<RequestListResponse>();
            if (status.search == null)
            {
                requests = await _context.Requests.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)).Select(a => new RequestListResponse
                {
                    id = a.Id,
                    code = a.Code,
                    request_name = a.RequestName,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                    },
                    service = new ServiceViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                    },
                    description = a.RequestDesciption,
                    priority = a.Priority,
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,


                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                requests = await _context.Requests.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)
                && (a.RequestStatus!.Equals(status.search)
                || a.RequestName!.Contains(status.search)
                || a.Code!.Contains(status.search))).Select(a => new RequestListResponse
                {
                    id = a.Id,
                    code = a.Code,
                    request_name = a.RequestName,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),

                    },
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                    },
                    service = new ServiceViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                    },
                    description = a.RequestDesciption,
                    priority = a.Priority,
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<RequestListResponse>(requests)
            {
                Total = requests.Count,
                Type = "Requests"
            };
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
                address = a.Address,
                mail = a.Mail,
                phone = a.Phone,
                description = a.Description,
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
                address = a.Address,
                mail = a.Mail,
                phone = a.Phone,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,


            }).ToListAsync();
            return new ObjectModelResponse(customer)
            {
                Type = "Customer"
            };
        }

        public async Task<ObjectModelResponse> GetServiceByCustomerId(Guid id)
        {
            var service = await _context.Customers.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new ListServiceResponse
            {
                service = _context.ContractServices.Where(x => x.Contract!.CustomerId.Equals(a.Id) && x.Contract.IsDelete == false
                && x.Contract.StartDate <= DateTime.Now && x.Contract.EndDate >= DateTime.Now).Select(x => new ServiceViewResponse
                {
                    id = x.ServiceId,
                    code = x.Service!.Code,
                    service_name = x.Service!.ServiceName,
                    description = x.Service!.Description,
                }).Distinct().ToList(),
            }).FirstOrDefaultAsync();
            return new ObjectModelResponse(service!)
            {
                Type = "Services"
            };
        }
        public async Task<ResponseModel<AgencyOfCustomerResponse>> GetAgenciesByCustomerId(Guid id)
        {
            var agencies = await _context.Agencies.Where(a => a.CustomerId.Equals(id) && a.IsDelete == false).Select(a => new AgencyOfCustomerResponse
            {
               id = a.Id,
               code = a.Code,
               agency_name = a.AgencyName,
               address = a.Address,
               phone = a.Telephone,
               manager_name = a.ManagerName,
               
            }).ToListAsync();
            return new ResponseModel<AgencyOfCustomerResponse>(agencies!)
            {
                Type = "Agencies",
                Total = agencies.Count,
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
                Mail = model.mail,
                Address = model.address,
                Phone = model.phone,
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
                        address = customer.Address,
                        mail = customer.Mail,
                        phone = customer.Phone,
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
                    address = customer.Address,
                    mail = customer.Mail,
                    phone = customer.Phone,
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
                Address = model.address,
                Mail = model.mail,
                Phone = model.phone,
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
                    address = customer.Address,
                    mail = customer.Mail,
                    phone = customer.Phone,
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
