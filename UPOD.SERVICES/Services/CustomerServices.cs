using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Numerics;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{
    public interface ICustomerService
    {
        Task<ResponseModel<CustomerResponse>> GetAll(PaginationRequest model, SearchRequest value);
        Task<ResponseModel<ContractResponse>> GetAllContractByCustomer(PaginationRequest model, SearchRequest value, Guid id);
        Task<ObjectModelResponse> CreateCustomer(CustomerRequest model);
        Task<ObjectModelResponse> GetCustomerDetails(Guid id);
        Task<ObjectModelResponse> UpdateCustomer(Guid id, CustomerRequest model);
        Task<ObjectModelResponse> DisableCustomer(Guid id);
        Task<ResponseModel<ServiceViewResponse>> GetServiceByCustomerId(Guid id);
        Task<ResponseModel<AgencyOfCustomerResponse>> GetAgenciesByCustomerId(Guid id);
        Task<ResponseModel<RequestResponse>> GetListRequestsByCustomerId(PaginationRequest model, SearchRequest value, Guid id);
        Task<ResponseModel<ServiceNotInContractViewResponse>> GetServiceNotInContractCustomerId(Guid id);
    }

    public class CustomerServices : ICustomerService
    {
        private readonly Database_UPODContext _context;
        public CustomerServices(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<ContractResponse>> GetAllContractByCustomer(PaginationRequest model, SearchRequest value, Guid id)
        {
            var total = await _context.Contracts.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)).ToListAsync();
            var contracts = new List<ContractResponse>();
            if (value.search == null)
            {
                total = await _context.Contracts.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)).ToListAsync();
                contracts = await _context.Contracts.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)).Select(a => new ContractResponse
                {
                    id = a.Id,
                    code = a.Code,
                    contract_name = a.ContractName,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),

                    },
                    start_date = a.StartDate,
                    end_date = a.EndDate,
                    is_delete = a.IsDelete,
                    is_expire = a.IsExpire,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    contract_price = a.ContractPrice,
                    description = a.Description,
                    attachment = a.Attachment,
                    img = a.Img,
                    service = _context.ContractServices.Where(x => x.ContractId.Equals(a.Id)).Select(x => new ServiceViewResponse
                    {
                        id = x.ServiceId,
                        code = x.Service!.Code,
                        service_name = x.Service!.ServiceName,
                        description = x.Service!.Description,
                        frequency_maintain = x.FrequencyMaintain,
                    }).ToList()

                }).OrderByDescending(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {

                var customer = await _context.Customers.Where(a => a.Name!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                total = await _context.Contracts.Where(a => a.IsDelete == false
                && a.CustomerId.Equals(id)
                 && (a.Code!.Contains(value.search.Trim())
                 || a.Code!.Contains(value.search.Trim())
                 || a.ContractName!.Contains(value.search.Trim())
                 || a.CustomerId!.Equals(customer))).ToListAsync();
                contracts = await _context.Contracts.Where(a => a.IsDelete == false
                && a.CustomerId.Equals(id)
                && (a.Code!.Contains(value.search.Trim())
                || a.Code!.Contains(value.search.Trim())
                || a.ContractName!.Contains(value.search.Trim())
                || a.CustomerId!.Equals(customer))).Select(a => new ContractResponse
                {
                    id = a.Id,
                    code = a.Code,
                    contract_name = a.ContractName,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),

                    },
                    start_date = a.StartDate,
                    end_date = a.EndDate,
                    is_delete = a.IsDelete,
                    is_expire = a.IsExpire,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    contract_price = a.ContractPrice,
                    description = a.Description,
                    attachment = a.Attachment,
                    img = a.Img,
                    service = _context.ContractServices.Where(x => x.ContractId.Equals(a.Id)).Select(x => new ServiceViewResponse
                    {
                        id = x.ServiceId,
                        code = x.Service!.Code,
                        service_name = x.Service!.ServiceName,
                        description = x.Service!.Description,
                        frequency_maintain = x.FrequencyMaintain,
                    }).ToList()

                }).OrderByDescending(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<ContractResponse>(contracts)
            {
                Total = total.Count,
                Type = "Contracts"
            };
        }

        public async Task<ResponseModel<RequestResponse>> GetListRequestsByCustomerId(PaginationRequest model, SearchRequest value, Guid id)
        {
            var total = await _context.Requests.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)).ToListAsync();
            var requests = new List<RequestResponse>();
            if (value.search == null)
            {
                total = await _context.Requests.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)).ToListAsync();
                requests = await _context.Requests.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)).Select(a => new RequestResponse
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
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    },
                    service = new ServiceViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                    },
                    reject_reason = a.ReasonReject,
                    description = a.RequestDesciption,
                    priority = a.Priority,
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    technicican = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    contract = new ContractViewResponse
                    {
                        id = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.ContractName).FirstOrDefault(),
                    }


                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Requests.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)
                && (a.RequestStatus!.Equals(value.search)
                || a.RequestName!.Contains(value.search)
                || a.Code!.Contains(value.search))).ToListAsync();
                requests = await _context.Requests.Where(a => a.IsDelete == false && a.CustomerId.Equals(id)
                && (a.RequestStatus!.Equals(value.search)
                || a.RequestName!.Contains(value.search)
                || a.Code!.Contains(value.search))).Select(a => new RequestResponse
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
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    },
                    service = new ServiceViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                    },
                    reject_reason = a.ReasonReject,
                    description = a.RequestDesciption,
                    priority = a.Priority,
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    technicican = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    contract = new ContractViewResponse
                    {
                        id = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.ContractName).FirstOrDefault(),
                    }

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<RequestResponse>(requests)
            {
                Total = total.Count,
                Type = "Requests"
            };
        }
        public async Task<ResponseModel<CustomerResponse>> GetAll(PaginationRequest model, SearchRequest value)
        {
            var total = await _context.Customers.Where(a => a.IsDelete == false).ToListAsync();
            var customers = new List<CustomerResponse>();
            if (value.search == null)
            {
                total = await _context.Customers.Where(a => a.IsDelete == false).ToListAsync();
                customers = await _context.Customers.Where(a => a.IsDelete == false).Select(a => new CustomerResponse
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
            }
            else
            {
                total = await _context.Customers.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search.Trim())
                || a.Name!.Contains(value.search.Trim())
                || a.Mail!.Contains(value.search.Trim())
                || a.Phone!.Contains(value.search.Trim()))).ToListAsync();
                customers = await _context.Customers.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search.Trim())
                || a.Name!.Contains(value.search.Trim())
                || a.Mail!.Contains(value.search.Trim())
                || a.Phone!.Contains(value.search.Trim()))).Select(a => new CustomerResponse
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
            }

            return new ResponseModel<CustomerResponse>(customers)
            {
                Total = total.Count,
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

        public async Task<ResponseModel<ServiceViewResponse>> GetServiceByCustomerId(Guid id)
        {

            var services = await _context.ContractServices.Where(x => x.Contract!.CustomerId.Equals(id) && x.Contract.IsDelete == false
                && x.Contract.StartDate <= DateTime.UtcNow.AddHours(7) && x.Contract.EndDate >= DateTime.UtcNow.AddHours(7)).Select(x => new ServiceViewResponse
                {
                    id = x.ServiceId,
                    code = x.Service!.Code,
                    service_name = x.Service!.ServiceName,
                    description = x.Service!.Description,
                }).Distinct().ToListAsync();
            var total = _context.ContractServices.Where(x => x.Contract!.CustomerId.Equals(id) && x.Contract.IsDelete == false
                && x.Contract.StartDate <= DateTime.UtcNow.AddHours(7) && x.Contract.EndDate >= DateTime.UtcNow.AddHours(7)).Distinct().ToList();
            return new ResponseModel<ServiceViewResponse>(services)
            {
                Total = total.Count,
                Type = "Services"
            };
        }
        public async Task<ResponseModel<ServiceNotInContractViewResponse>> GetServiceNotInContractCustomerId(Guid id)
        {

            var services_in_contract = await _context.ContractServices.Where(x => x.Contract!.CustomerId.Equals(id) && x.Contract.IsDelete == false
                && x.Contract.StartDate <= DateTime.UtcNow.AddHours(7) && x.Contract.EndDate >= DateTime.UtcNow.AddHours(7)).Select(a => new ServiceNotInContractViewResponse
                {
                    id = a.ServiceId,
                    service_name = a.Service!.ServiceName,
                    code = a.Service!.Code,
                }).Distinct().ToListAsync();
            var list_services = await _context.Services.Where(x => x.IsDelete == false).Select(a => new ServiceNotInContractViewResponse
            {
                id = a.Id,
                service_name = a.ServiceName,
                code = a.Code,
            }).Distinct().ToListAsync();
            var sv = new List<ServiceNotInContractViewResponse>();
            var rs = list_services.Except(services_in_contract).ToList();
            var listService = new List<ServiceNotInContractViewResponse>();
            foreach (var service in rs)
            {
                listService.Add(new ServiceNotInContractViewResponse
                {
                    id = service.id,
                    code = service.code,
                    service_name = service.service_name,
                });
            }
            var total = rs;
            return new ResponseModel<ServiceNotInContractViewResponse>(listService)
            {
                Total = total.Count,
                Type = "Services"
            };
        }
        public async Task<ResponseModel<AgencyOfCustomerResponse>> GetAgenciesByCustomerId(Guid id)
        {
            var total = await _context.Agencies.Where(a => a.CustomerId.Equals(id) && a.IsDelete == false).ToListAsync();
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
                Total = total.Count,
            };
        }
        public async Task<ObjectModelResponse> CreateCustomer(CustomerRequest model)
        {
            var customer_id = Guid.NewGuid();
            while (true)
            {
                var customer_dup = await _context.Customers.Where(x => x.Id.Equals(customer_id)).FirstOrDefaultAsync();
                if (customer_dup == null)
                {
                    break;
                }
                else
                {
                    customer_id = Guid.NewGuid();
                }
            }
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("CU", code_number + 1);
            var customer = new Customer
            {
                Id = customer_id,
                Code = code,
                Name = model.name,
                AccountId = model.account_id,
                Description = model.description,
                Mail = model.mail,
                Address = model.address,
                Phone = model.phone,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7),
                IsDelete = false
            };
            var account_asign = await _context.Accounts.Where(a => a.Id.Equals(model.account_id)).FirstOrDefaultAsync();
            account_asign!.IsAssign = true;
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
                status = 200;
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
            var agencies = await _context.Agencies.Where(x => x.CustomerId.Equals(id)).ToListAsync();
            var contracts = await _context.Contracts.Where(x => x.CustomerId.Equals(id)).ToListAsync();
            customer!.IsDelete = true;
            foreach (var item in agencies)
            {
                item.IsDelete = true;
                var devices = await _context.Devices.Where(x => x.AgencyId.Equals(item.Id)).ToListAsync();
                foreach (var item1 in devices)
                {
                    item1.IsDelete = true;
                }
            }
            foreach (var item in contracts)
            {
                item.IsDelete = true;
            }
            customer.UpdateDate = DateTime.UtcNow.AddHours(7);
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
                UpdateDate = DateTime.UtcNow.AddHours(7),

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
