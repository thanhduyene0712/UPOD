using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Enum;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{

    public interface ITechnicianService
    {
        Task<ResponseModel<TechnicianResponse>> GetListTechnicians(PaginationRequest model, SearchRequest value);
        Task<ObjectModelResponse> GetDetailsTechnician(Guid id);
        Task<ObjectModelResponse> CreateTechnician(TechnicianRequest model);
        Task<ObjectModelResponse> UpdateTechnician(Guid id, TechnicianUpdateRequest model);
        Task<ObjectModelResponse> DisableTechnician(Guid id);
        Task<ResponseModel<DevicesOfRequestResponse>> CreateTicket(Guid id, ListTicketRequest model);
        //Task<ResponseModel<DevicesOfRequestResponse>> AddTicket(Guid id, ListTicketRequest model);
        Task<ResponseModel<RequestResponse>> GetListRequestsOfTechnician(PaginationRequest model, Guid id, FilterStatusRequest value);
        Task<ResponseModel<DevicesOfRequestResponse>> GetDevicesByRequest(PaginationRequest model, Guid id);
        Task<ObjectModelResponse> ResolvingRequest(Guid id);
        //Task<ObjectModelResponse> ConfirmRequest(Guid id);
        Task<ObjectModelResponse> UpdateDeviceTicket(Guid id, ListTicketRequest model);
        Task<ObjectModelResponse> DisableDeviceOfTicket(Guid id);
    }

    public class TechnicianServices : ITechnicianService
    {
        private readonly Database_UPODContext _context;
        public TechnicianServices(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<DevicesOfRequestResponse>> GetDevicesByRequest(PaginationRequest model, Guid id)
        {
            var total = await _context.Tickets.Where(a => a.RequestId.Equals(id) && a.IsDelete == false).ToListAsync();
            var device_of_request = await _context.Tickets.Where(a => a.RequestId.Equals(id) && a.IsDelete == false).Select(a => new DevicesOfRequestResponse
            {
                ticket_id = a.Id,
                device_id = a.Device!.Id,
                code = a.Device.Code,
                name = a.Device.DeviceName,
                solution = a.Solution,
                description = a.Description,
                create_date = a.CreateDate,
                img = _context.Images.Where(x => x.CurrentObject_Id.Equals(a.Id)).Select(a => a.Link).ToList()!,

            }).OrderByDescending(x => x.code).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<DevicesOfRequestResponse>(device_of_request)
            {
                Total = total.Count,
                Type = "Devices"
            };
        }
        public async Task<ResponseModel<TechnicianResponse>> GetListTechnicians(PaginationRequest model, SearchRequest value)
        {
            var total = await _context.Technicians.Where(a => a.IsDelete == false).ToListAsync();
            var technicians = new List<TechnicianResponse>();
            if (value.search == null)
            {
                total = await _context.Technicians.Where(a => a.IsDelete == false).ToListAsync();
                technicians = await _context.Technicians.Where(a => a.IsDelete == false).Select(a => new TechnicianResponse
                {
                    id = a.Id,
                    code = a.Code,
                    area = new AreaViewResponse
                    {
                        id = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.Code).FirstOrDefault(),
                        area_name = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                        description = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    technician_name = a.TechnicianName,
                    account = new AccountViewResponse
                    {
                        id = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Code).FirstOrDefault(),
                        role_name = _context.Roles.Where(x => x.Id.Equals(a.Account!.RoleId)).Select(x => x.RoleName).FirstOrDefault(),
                        username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
                        password = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Password).FirstOrDefault(),
                    },
                    telephone = a.Telephone,
                    email = a.Email,
                    gender = a.Gender,
                    address = a.Address,
                    is_busy = a.IsBusy,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    service = _context.Skills.Where(x => x.TechnicianId.Equals(a.Id)).Select(a => new ServiceViewResponse
                    {
                        id = a.TechnicianId,
                        code = a.Service!.Code,
                        service_name = a.Service.ServiceName,
                        description = a.Service.Description,
                    }).ToList(),
                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Technicians.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search.Trim())
                || a.TechnicianName!.Contains(value.search.Trim())
                || a.Email!.Contains(value.search.Trim())
                || a.Telephone!.Contains(value.search.Trim())
                || a.Address!.Contains(value.search.Trim()))).ToListAsync();
                technicians = await _context.Technicians.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search.Trim())
                || a.TechnicianName!.Contains(value.search.Trim())
                || a.Email!.Contains(value.search.Trim())
                || a.Telephone!.Contains(value.search.Trim())
                || a.Address!.Contains(value.search.Trim()))).Select(a => new TechnicianResponse
                {
                    id = a.Id,
                    code = a.Code,
                    area = new AreaViewResponse
                    {
                        id = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.Code).FirstOrDefault(),
                        area_name = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                        description = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    technician_name = a.TechnicianName,
                    account = new AccountViewResponse
                    {
                        id = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Code).FirstOrDefault(),
                        role_name = _context.Roles.Where(x => x.Id.Equals(a.Account!.RoleId)).Select(x => x.RoleName).FirstOrDefault(),
                        username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
                        password = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Password).FirstOrDefault(),
                    },
                    telephone = a.Telephone,
                    email = a.Email,
                    gender = a.Gender,
                    address = a.Address,
                    is_busy = a.IsBusy,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    service = _context.Skills.Where(x => x.TechnicianId.Equals(a.Id)).Select(a => new ServiceViewResponse
                    {
                        id = a.TechnicianId,
                        code = a.Service!.Code,
                        service_name = a.Service.ServiceName,
                        description = a.Service.Description,
                    }).ToList(),
                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            return new ResponseModel<TechnicianResponse>(technicians)
            {
                Total = total.Count,
                Type = "Technicians"
            };
        }
        public async Task<ObjectModelResponse> ConfirmRequest(Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false && a.RequestStatus!.Equals("EDITING")).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(x => x.Id.Equals(request!.CurrentTechnicianId)).FirstOrDefaultAsync();
            technician!.IsBusy = false;
            request!.RequestStatus = ProcessStatus.RESOLVED.ToString();
            request!.UpdateDate = DateTime.UtcNow.AddHours(7);
            _context.Requests.Update(request);
            var data = new ResolvingRequestResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new ResolvingRequestResponse
                {
                    id = request.Id,
                    code = request.Code,
                    name = request.RequestName,
                    status = request.RequestStatus,
                    start_time = request.StartTime,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Request"
            };
        }
        public async Task<ResponseModel<RequestResponse>> GetListRequestsOfTechnician(PaginationRequest model, Guid id, FilterStatusRequest value)
        {
            var total = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)
            && (a.RequestStatus!.Equals("EDITING")
            || a.RequestStatus!.Equals("PREPARING")
            || a.RequestStatus!.Equals("RESOLVING")
            || a.RequestStatus!.Equals("RESOLVED"))).ToListAsync();
            var requests = new List<RequestResponse>();
            if (value.search == null && value.status == null)
            {
                total = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)
            && (a.RequestStatus!.Equals("EDITING")
            || a.RequestStatus!.Equals("PREPARING")
            || a.RequestStatus!.Equals("RESOLVING")
            || a.RequestStatus!.Equals("RESOLVED"))).ToListAsync();
                requests = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)
            && (a.RequestStatus!.Equals("EDITING")
            || a.RequestStatus!.Equals("PREPARING")
            || a.RequestStatus!.Equals("RESOLVING")
            || a.RequestStatus!.Equals("RESOLVED"))).Select(a => new RequestResponse
                {
                    id = a.Id,
                    code = a.Code,
                    request_name = a.RequestName,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
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
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            else
            {
                if (value.search == null)
                {
                    value.search = "";
                }
                if (value.status == null)
                {
                    value.status = "";
                }
                if (value.search.ToLower().Trim().Contains("admin"))
                {
                    total = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(id)
                    && (a.RequestStatus!.Contains(value.status!.Trim())
                    && a.AdminId != null)).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                   && a.CurrentTechnicianId.Equals(id)
                    && (a.RequestStatus!.Contains(value.status!.Trim())
                    && a.AdminId != null)).Select(a => new RequestResponse
                    {
                        id = a.Id,
                        code = a.Code,
                        request_name = a.RequestName,
                        customer = new CustomerViewResponse
                        {
                            id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                            code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                            cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                            description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                            phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                            address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                            mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
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
                        contract = new ContractViewResponse
                        {
                            id = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Id).FirstOrDefault(),
                            code = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Code).FirstOrDefault(),
                            name = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.ContractName).FirstOrDefault(),
                        },
                        reject_reason = a.ReasonReject,
                        description = a.RequestDesciption,
                        request_status = a.RequestStatus,
                        create_date = a.CreateDate,
                        update_date = a.UpdateDate,
                        technicican = new TechnicianViewResponse
                        {
                            id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                            phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                            email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                            code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                            tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                        }
                    }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
                }
                else
                {
                    var agency_name = await _context.Agencies.Where(a => a.AgencyName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    var customer_name = await _context.Customers.Where(a => a.Name!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    var contract_name = await _context.Contracts.Where(a => a.ContractName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    var service_name = await _context.Services.Where(a => a.ServiceName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    total = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(id)
                    && (a.RequestStatus!.Contains(value.status!.Trim())
                    && (a.RequestName!.Contains(value.search!.Trim())
                    || a.Code!.Contains(value.search!.Trim())
                    || a.RequestDesciption!.Contains(value.search!.Trim())
                    || a.AgencyId!.Equals(agency_name)
                    || a.CustomerId!.Equals(customer_name)
                    || a.ContractId!.Equals(contract_name)
                    || a.ServiceId!.Equals(service_name)))).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(id)
                    && (a.RequestName!.Contains(value.search!.Trim())
                    || a.Code!.Contains(value.search.Trim())
                    || a.RequestDesciption!.Contains(value.search!.Trim())
                    || a.AgencyId!.Equals(agency_name)
                    || a.CustomerId!.Equals(customer_name)
                    || a.ContractId!.Equals(contract_name)
                    || a.ServiceId!.Equals(service_name))
                    && (a.RequestStatus!.Contains(value.status!.Trim()))).Select(a => new RequestResponse
                    {
                        id = a.Id,
                        code = a.Code,
                        request_name = a.RequestName,
                        customer = new CustomerViewResponse
                        {
                            id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                            code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                            cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                            description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                            phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                            address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                            mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
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
                        contract = new ContractViewResponse
                        {
                            id = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Id).FirstOrDefault(),
                            code = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Code).FirstOrDefault(),
                            name = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.ContractName).FirstOrDefault(),
                        },
                        reject_reason = a.ReasonReject,
                        description = a.RequestDesciption,
                        request_status = a.RequestStatus,
                        create_date = a.CreateDate,
                        update_date = a.UpdateDate,
                        technicican = new TechnicianViewResponse
                        {
                            id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                            phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                            email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                            code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                            tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                        }
                    }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
                }
            }
            return new ResponseModel<RequestResponse>(requests)
            {
                Total = total.Count,
                Type = "Request"
            };
        }
        public async Task<ObjectModelResponse> GetDetailsTechnician(Guid id)
        {

            var technician = await _context.Technicians.Where(a => a.IsDelete == false && a.Id.Equals(id)).Select(a => new TechnicianResponse
            {
                id = a.Id,
                code = a.Code,
                area = new AreaViewResponse
                {
                    id = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.Code).FirstOrDefault(),
                    area_name = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                    description = _context.Areas.Where(x => x.Id.Equals(a.AreaId)).Select(x => x.Description).FirstOrDefault(),
                },
                technician_name = a.TechnicianName,
                account = new AccountViewResponse
                {
                    id = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Code).FirstOrDefault(),
                    role_name = _context.Roles.Where(x => x.Id.Equals(a.Account!.RoleId)).Select(x => x.RoleName).FirstOrDefault(),
                    username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
                    password = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Password).FirstOrDefault(),
                },
                telephone = a.Telephone,
                email = a.Email,
                gender = a.Gender,
                address = a.Address,
                is_busy = a.IsBusy,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
                service = _context.Skills.Where(x => x.TechnicianId.Equals(a.Id)).Select(a => new ServiceViewResponse
                {
                    id = a.ServiceId,
                    code = a.Service!.Code,
                    service_name = a.Service.ServiceName,
                    description = a.Service.Description,
                }).ToList(),
            }).FirstOrDefaultAsync();
            return new ObjectModelResponse(technician!)
            {
                Type = "Technician"
            };
        }
        private async Task<int> GetLastCode()
        {
            var technician = await _context.Technicians.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(technician!.Code!);
        }

        public async Task<ResponseModel<DevicesOfRequestResponse>> CreateTicket(Guid id, ListTicketRequest model)
        {

            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(x => x.Id.Equals(request!.CurrentTechnicianId)).FirstOrDefaultAsync();
            technician!.IsBusy = false;
            request!.RequestStatus = ProcessStatus.RESOLVED.ToString();
            request.EndTime = DateTime.UtcNow.AddHours(7);
            _context.Requests.Update(request);
            _context.Technicians.Update(technician);
            var list = new List<DevicesOfRequestResponse>();

            foreach (var item in model.ticket)
            {
                var device_id = Guid.NewGuid();
                while (true)
                {
                    var ticket_id = await _context.Tickets.Where(x => x.Id.Equals(device_id)).FirstOrDefaultAsync();
                    if (ticket_id == null)
                    {
                        break;
                    }
                    else
                    {
                        device_id = Guid.NewGuid();
                    }
                }
                var ticket = new Ticket
                {
                    Id = device_id,
                    RequestId = request.Id,
                    DeviceId = item.device_id,
                    Description = item.description,
                    Solution = item.solution,
                    IsDelete = false,
                    CreateBy = technician!.Id,
                    CreateDate = DateTime.UtcNow.AddHours(7),
                    UpdateDate = DateTime.UtcNow.AddHours(7)
                };


                foreach (var item1 in item.img!)
                {
                    var img_id = Guid.NewGuid();
                    while (true)
                    {
                        var img_dup = await _context.Images.Where(x => x.Id.Equals(img_id)).FirstOrDefaultAsync();
                        if (img_dup == null)
                        {
                            break;
                        }
                        else
                        {
                            img_id = Guid.NewGuid();
                        }
                    }
                    var imgTicket = new Image
                    {
                        Id = img_id,
                        Link = item1,
                        CurrentObject_Id = ticket.Id,
                    };
                    await _context.Tickets.AddAsync(ticket);
                    await _context.Images.AddAsync(imgTicket);

                }

                await _context.SaveChangesAsync();
                list.Add(new DevicesOfRequestResponse
                {
                    ticket_id = ticket.Id,
                    device_id = ticket.DeviceId,
                    code = _context.Devices.Where(a => a.Id.Equals(ticket.DeviceId)).Select(a => a.Code).FirstOrDefault(),
                    name = _context.Devices.Where(a => a.Id.Equals(ticket.DeviceId)).Select(a => a.DeviceName).FirstOrDefault(),
                    solution = ticket.Solution,
                    description = ticket.Description,
                    create_date = ticket.CreateDate,
                    img = _context.Images.Where(a => a.CurrentObject_Id.Equals(ticket.Id)).Select(x => x.Link).ToList()!,
                });

            }
            return new ResponseModel<DevicesOfRequestResponse>(list)
            {
                Total = list.Count,
                Type = "Devices"
            };
        }

        public async Task<ObjectModelResponse> UpdateDeviceTicket(Guid id, ListTicketRequest model)
        {

            var devices = await _context.Tickets.Where(a => a.RequestId.Equals(id) && a.IsDelete == false).ToListAsync();
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(x => x.Id.Equals(request!.CurrentTechnicianId)).FirstOrDefaultAsync();
            technician!.IsBusy = false;
            request!.RequestStatus = ProcessStatus.RESOLVED.ToString();
            var list = new List<DevicesOfRequestResponse>();
            foreach (var device in devices)
            {
                _context.Tickets.Remove(device);
                var imgs = await _context.Images.Where(a => a.CurrentObject_Id.Equals(device.Id)).ToListAsync();
                foreach (var item in imgs)
                {
                    _context.Images.Remove(item);
                }
            }
            foreach (var item in model.ticket)
            {
                var device_id = Guid.NewGuid();
                while (true)
                {
                    var ticket_id = await _context.Tickets.Where(x => x.Id.Equals(device_id)).FirstOrDefaultAsync();
                    if (ticket_id == null)
                    {
                        break;
                    }
                    else
                    {
                        device_id = Guid.NewGuid();
                    }
                }
                var ticket = new Ticket
                {
                    Id = device_id,
                    RequestId = request!.Id,
                    DeviceId = item.device_id,
                    Description = item.description,
                    Solution = item.solution,
                    IsDelete = false,
                    CreateBy = technician!.Id,
                    CreateDate = DateTime.UtcNow.AddHours(7),
                    UpdateDate = DateTime.UtcNow.AddHours(7)
                };
                await _context.Tickets.AddAsync(ticket);

                foreach (var item1 in item.img!)
                {
                    var img_id = Guid.NewGuid();
                    while (true)
                    {
                        var img_dup = await _context.Images.Where(x => x.Id.Equals(img_id)).FirstOrDefaultAsync();
                        if (img_dup == null)
                        {
                            break;
                        }
                        else
                        {
                            img_id = Guid.NewGuid();
                        }
                    }
                    var imgTicket = new Image
                    {
                        Id = img_id,
                        Link = item1,
                        CurrentObject_Id = ticket.Id,
                    };
                    await _context.Images.AddAsync(imgTicket);

                }

                await _context.SaveChangesAsync();
                list.Add(new DevicesOfRequestResponse
                {
                    ticket_id = ticket.Id,
                    device_id = ticket.DeviceId,
                    code = _context.Devices.Where(a => a.Id.Equals(ticket.DeviceId)).Select(a => a.Code).FirstOrDefault(),
                    name = _context.Devices.Where(a => a.Id.Equals(ticket.DeviceId)).Select(a => a.DeviceName).FirstOrDefault(),
                    solution = ticket.Solution,
                    description = ticket.Description,
                    create_date = ticket.CreateDate,
                    img = _context.Images.Where(a => a.CurrentObject_Id.Equals(ticket.Id)).Select(x => x.Link).ToList()!,
                });

            }
            return new ObjectModelResponse(list!)
            {
                Status = 201,
                Type = "Device"
            };
        }
        public async Task<ObjectModelResponse> CreateTechnician(TechnicianRequest model)
        {
            var num = await GetLastCode();
            var code = CodeHelper.GeneratorCode("TE", num + 1);
            while (true)
            {
                var code_dup = await _context.Technicians.Where(a => a.Code.Equals(code)).FirstOrDefaultAsync();
                if (code_dup == null)
                {
                    break;
                }
                else
                {
                    code = "TE-" + num++.ToString();
                }
            }
            var technician_id = Guid.NewGuid();
            while (true)
            {
                var technician_dup = await _context.Technicians.Where(x => x.Id.Equals(technician_id)).FirstOrDefaultAsync();
                if (technician_dup == null)
                {
                    break;
                }
                else
                {
                    technician_id = Guid.NewGuid();
                }
            }
            var technician = new Technician
            {
                Id = technician_id,
                Code = code,
                AreaId = model.area_id,
                TechnicianName = model.technician_name,
                AccountId = model.account_id,
                Telephone = model.telephone,
                Address = model.address,
                Email = model.email,
                Gender = model.gender,
                IsBusy = false,
                IsDelete = false,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7)
            };
            var account_asign = await _context.Accounts.Where(a => a.Id.Equals(model.account_id)).FirstOrDefaultAsync();
            account_asign!.IsAssign = true;
            foreach (var item in model.service_id)
            {
                var skill_id = Guid.NewGuid();
                while (true)
                {
                    var skill_dup = await _context.Skills.Where(x => x.Id.Equals(skill_id)).FirstOrDefaultAsync();
                    if (skill_dup == null)
                    {
                        break;
                    }
                    else
                    {
                        skill_id = Guid.NewGuid();
                    }
                }
                var skill = new Skill
                {
                    Id = skill_id,
                    TechnicianId = technician.Id,
                    ServiceId = item,
                    IsDelete = false,
                };
                _context.Skills.Add(skill);
            }
            var data = new TechnicianUpdateResponse();
            await _context.Technicians.AddAsync(technician);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new TechnicianUpdateResponse()
                {
                    id = technician!.Id,
                    code = technician!.Code,
                    area_id = technician.AreaId,
                    technician_name = technician.TechnicianName,
                    account_id = technician.AccountId,
                    telephone = technician.Telephone,
                    email = technician.Email,
                    gender = technician.Gender,
                    address = technician.Address,
                    is_busy = technician.IsBusy,
                    is_delete = technician.IsDelete,
                    create_date = technician.CreateDate,
                    update_date = technician.UpdateDate,
                    service_id = _context.Skills.Where(a => a.TechnicianId.Equals(technician.Id)).Select(a => a.ServiceId).ToList(),

                };
            }


            return new ObjectModelResponse(data)
            {
                Type = "Technician"
            };
        }


        public async Task<ObjectModelResponse> DisableTechnician(Guid id)
        {
            var technician = await _context.Technicians.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            technician!.IsDelete = true;
            technician.UpdateDate = DateTime.UtcNow.AddHours(7);
            var data = new TechnicianUpdateResponse();
            _context.Technicians.Update(technician);
            var technician_default = await _context.Agencies.Where(a => a.TechnicianId.Equals(id)).ToListAsync();
            var message = "Susscessfull";
            var status = 201;
            var technician_request = await _context.Requests.Where(a => a.CurrentTechnicianId.Equals(id) && a.RequestStatus!.Equals("EDITING")).ToListAsync();
            if (technician_request.Count > 0)
            {
                foreach (var item in technician_request)
                {
                    technician!.IsDelete = false;
                    message = "You can't delete this technician";
                    _context.Requests.Update(item);
                    status = 400;
                }
            }
            else
            {
                var technician_agency = await _context.Requests.Where(a => a.CurrentTechnicianId.Equals(id) && a.RequestStatus != "EDITING").ToListAsync();
                foreach (var item in technician_agency)
                {
                    status = 201;
                    if (item.RequestStatus!.Equals("PREPARING") || item.RequestStatus!.Equals("RESOLVING"))
                    {
                        item.CurrentTechnicianId = null;
                        item.RequestStatus = ProcessStatus.PENDING.ToString();
                        _context.Requests.Update(item);
                        foreach (var item1 in technician_default)
                        {
                            item1.TechnicianId = null;
                            _context.Agencies.Update(item1);
                        }
                    }
                    else
                    {
                        foreach (var item1 in technician_default)
                        {
                            item1.TechnicianId = null;
                            _context.Agencies.Update(item1);
                        }
                    }
                }
            }


            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new TechnicianUpdateResponse
                {
                    id = technician!.Id,
                    code = technician!.Code,
                    area_id = technician.AreaId,
                    technician_name = technician.TechnicianName,
                    account_id = technician.AccountId,
                    telephone = technician.Telephone,
                    email = technician.Email,
                    gender = technician.Gender,
                    address = technician.Address,
                    is_busy = technician.IsBusy,
                    is_delete = technician.IsDelete,
                    create_date = technician.CreateDate,
                    update_date = technician.UpdateDate,
                    service_id = _context.Skills.Where(a => a.TechnicianId.Equals(technician.Id)).Select(a => a.ServiceId).ToList(),
                };
            }

            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
                Type = "Technician"
            };
        }
        public async Task<ObjectModelResponse> DisableDeviceOfTicket(Guid id)
        {
            var ticket = await _context.Tickets.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            ticket!.IsDelete = true;
            ticket.UpdateDate = DateTime.UtcNow.AddHours(7);
            var data = new TicketViewResponse();
            _context.Tickets.Update(ticket);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new TicketViewResponse
                {
                    id = ticket!.Id,
                    device_id = ticket!.DeviceId,
                    code = _context.Devices.Where(a => a.Id.Equals(ticket!.DeviceId)).Select(a => a.Code).FirstOrDefault(),
                    solution = ticket.Solution,
                    description = ticket.Description
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Device"
            };
        }
        public async Task<ObjectModelResponse> ResolvingRequest(Guid id)
        {
            var request = await _context.Requests.Where(x => x.Id.Equals(id) && x.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(x => x.Id.Equals(request!.CurrentTechnicianId)).FirstOrDefaultAsync();
            technician!.IsBusy = true;
            request!.RequestStatus = ProcessStatus.RESOLVING.ToString();
            request.StartTime = DateTime.UtcNow.AddHours(7);
            _context.Requests.Update(request);
            _context.Technicians.Update(technician);
            var data = new ResolvingRequestResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new ResolvingRequestResponse
                {
                    id = id,
                    code = request.Code,
                    status = request.RequestStatus,
                    name = request.RequestName,
                    start_time = request.StartTime,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Request"
            };
        }
        public async Task<ObjectModelResponse> UpdateTechnician(Guid id, TechnicianUpdateRequest model)
        {
            var technician = await _context.Technicians.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(x => new Technician
            {
                Id = id,
                Code = x.Code,
                AreaId = model.area_id,
                TechnicianName = model.technician_name,
                AccountId = x.AccountId,
                Telephone = model.telephone,
                Address = model.address,
                Email = model.email,
                Gender = model.gender,
                IsBusy = x.IsBusy,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.UtcNow.AddHours(7)
            }).FirstOrDefaultAsync();
            var skill_remove = await _context.Skills.Where(a => a.TechnicianId.Equals(id)).ToListAsync();
            foreach (var item in skill_remove)
            {
                _context.Skills.Remove(item);
            }
            foreach (var item in model.service_id)
            {
                var skill = new Skill
                {
                    Id = Guid.NewGuid(),
                    TechnicianId = technician!.Id,
                    ServiceId = item,
                    IsDelete = false,
                };
                _context.Skills.Add(skill);
            }
            var data = new TechnicianUpdateResponse();
            _context.Technicians.Update(technician!);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new TechnicianUpdateResponse
                {
                    id = technician!.Id,
                    code = technician!.Code,
                    area_id = technician.AreaId,
                    technician_name = technician.TechnicianName,
                    account_id = technician.AccountId,
                    telephone = technician.Telephone,
                    email = technician.Email,
                    gender = technician.Gender,
                    address = technician.Address,
                    is_busy = technician.IsBusy,
                    is_delete = technician.IsDelete,
                    create_date = technician.CreateDate,
                    update_date = technician.UpdateDate,
                    service_id = _context.Skills.Where(a => a.TechnicianId.Equals(technician.Id)).Select(a => a.ServiceId).ToList(),
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Technician"
            };
        }


    }
}
