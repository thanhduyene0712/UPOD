﻿using Microsoft.EntityFrameworkCore;
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
        Task<ResponseModel<RequestResponse>> GetListRequestsOfTechnician(PaginationRequest model, Guid id, FilterStatusRequest value);
        Task<ResponseModel<DevicesOfRequestResponse>> GetDevicesByRequest(PaginationRequest model, Guid id);
        Task<ObjectModelResponse> ResolvingRequest(Guid id, Guid tech_id);
        Task<ObjectModelResponse> RejectRequest(Guid id, Guid tech_id);
        Task<ObjectModelResponse> UpdateDeviceTicket(Guid id, ListTicketRequest model);
        Task<ObjectModelResponse> IsBusyTechnician(Guid id, IsBusyRequest model);
        Task<ObjectModelResponse> DisableDeviceOfTicket(Guid id);
        Task<ResponseModel<Technician>> ResetBreachTechnician();
        Task<ResponseModel<RequestResponse>> GetListRequestsOfTechnicianAgency(PaginationRequest model, Guid tech_id, Guid agency_id, FilterStatusRequest value);
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
                img = _context.Images.Where(x => x.CurrentObject_Id.Equals(a.Id) && x.ObjectName!.Equals(ObjectName.TI.ToString())).Select(a => a.Link).ToList()!,

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
                && (a.Code!.Contains(value.search)
                || a.TechnicianName!.Contains(value.search)
                || a.Email!.Contains(value.search)
                || a.Telephone!.Contains(value.search)
                || a.Address!.Contains(value.search))).ToListAsync();
                technicians = await _context.Technicians.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search)
                || a.TechnicianName!.Contains(value.search)
                || a.Email!.Contains(value.search)
                || a.Telephone!.Contains(value.search)
                || a.Address!.Contains(value.search))).Select(a => new TechnicianResponse
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
                    technician = request.CurrentTechnician!.TechnicianName,
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
            && (a.RequestStatus!.Equals("WARNING")
            || a.RequestStatus!.Equals("PREPARING")
            || a.RequestStatus!.Equals("RESOLVING")
            || a.RequestStatus!.Equals("RESOLVED")
            || a.RequestStatus!.Equals("COMPLETED"))).ToListAsync();
            var requests = new List<RequestResponse>();
            if (value.search == null && value.status == null)
            {
                total = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)
            && (a.RequestStatus!.Equals("WARNING")
            || a.RequestStatus!.Equals("PREPARING")
            || a.RequestStatus!.Equals("RESOLVING")
            || a.RequestStatus!.Equals("RESOLVED")
            || a.RequestStatus!.Equals("COMPLETED"))).ToListAsync();
                requests = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)
            && (a.RequestStatus!.Equals("WARNING")
            || a.RequestStatus!.Equals("PREPARING")
            || a.RequestStatus!.Equals("RESOLVING")
            || a.RequestStatus!.Equals("RESOLVED")
            || a.RequestStatus!.Equals("COMPLETED"))).Select(a => new RequestResponse
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
                    phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
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
                description = a.RequestDesciption,
                admin_id = a.AdminId,
                contract = new ContractViewResponse
                {
                    id = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Id).FirstOrDefault(),
                    code = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Code).FirstOrDefault(),
                    name = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.ContractName).FirstOrDefault(),
                },
                reject_reason = a.ReasonReject,
                technicican = new TechnicianViewResponse
                {
                    id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                    phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                    email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                    code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                    tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                },
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
                if (value.search.ToLower().Contains("admin"))
                {
                    total = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(id)
                    && (a.RequestStatus!.Contains(value.status!)
                    && a.AdminId != null)
                    && (a.RequestStatus!.Equals("WARNING")
                    || a.RequestStatus!.Equals("PREPARING")
                    || a.RequestStatus!.Equals("RESOLVING")
                    || a.RequestStatus!.Equals("RESOLVED")
                    || a.RequestStatus!.Equals("COMPLETED"))).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                   && a.CurrentTechnicianId.Equals(id)
                    && (a.RequestStatus!.Contains(value.status!)
                    && a.AdminId != null)
                    && (a.RequestStatus!.Equals("WARNING")
                    || a.RequestStatus!.Equals("PREPARING")
                    || a.RequestStatus!.Equals("RESOLVING")
                    || a.RequestStatus!.Equals("RESOLVED")
                    || a.RequestStatus!.Equals("COMPLETED"))).Select(a => new RequestResponse
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
                            phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
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
                    var agency_name = await _context.Agencies.Where(a => a.AgencyName!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                    var customer_name = await _context.Customers.Where(a => a.Name!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                    var contract_name = await _context.Contracts.Where(a => a.ContractName!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                    var service_name = await _context.Services.Where(a => a.ServiceName!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                    total = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(id)
                    && (a.RequestStatus!.Contains(value.status!)
                    && (a.RequestName!.Contains(value.search!)
                    || a.Code!.Contains(value.search!)
                    || a.RequestDesciption!.Contains(value.search!)
                    || a.AgencyId!.Equals(agency_name)
                    || a.CustomerId!.Equals(customer_name)
                    || a.ContractId!.Equals(contract_name)
                    || a.ServiceId!.Equals(service_name))
                    && (a.RequestStatus!.Equals("WARNING")
                    || a.RequestStatus!.Equals("PREPARING")
                    || a.RequestStatus!.Equals("RESOLVING")
                    || a.RequestStatus!.Equals("RESOLVED")
                    || a.RequestStatus!.Equals("COMPLETED")))).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(id)
                    && (a.RequestName!.Contains(value.search!)
                    || a.Code!.Contains(value.search)
                    || a.RequestDesciption!.Contains(value.search!)
                    || a.AgencyId!.Equals(agency_name)
                    || a.CustomerId!.Equals(customer_name)
                    || a.ContractId!.Equals(contract_name)
                    || a.ServiceId!.Equals(service_name))
                    && (a.RequestStatus!.Contains(value.status!))
                    && (a.RequestStatus!.Equals("WARINING")
                    || a.RequestStatus!.Equals("PREPARING")
                    || a.RequestStatus!.Equals("RESOLVING")
                    || a.RequestStatus!.Equals("RESOLVED")
                    || a.RequestStatus!.Equals("COMPLETED"))).Select(a => new RequestResponse
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
                            phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
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
        public async Task<ResponseModel<RequestResponse>> GetListRequestsOfTechnicianAgency(PaginationRequest model, Guid tech_id, Guid agency_id, FilterStatusRequest value)
        {
            var total = await _context.Requests.Where(a => a.IsDelete == false
            && a.CurrentTechnicianId.Equals(tech_id)
            && a.AgencyId.Equals(agency_id)
            && (a.RequestStatus!.Equals("WARNING")
            || a.RequestStatus!.Equals("PREPARING")
            || a.RequestStatus!.Equals("RESOLVING")
            || a.RequestStatus!.Equals("RESOLVED")
            || a.RequestStatus!.Equals("COMPLETED"))).ToListAsync();
            var requests = new List<RequestResponse>();
            if (value.search == null && value.status == null)
            {
                total = await _context.Requests.Where(a => a.IsDelete == false
                && a.CurrentTechnicianId.Equals(tech_id)
                && a.AgencyId.Equals(agency_id)
                && (a.RequestStatus!.Equals("WARNING")
                || a.RequestStatus!.Equals("PREPARING")
                || a.RequestStatus!.Equals("RESOLVING")
                || a.RequestStatus!.Equals("RESOLVED")
                || a.RequestStatus!.Equals("COMPLETED"))).ToListAsync();
                requests = await _context.Requests.Where(a => a.IsDelete == false
                && a.CurrentTechnicianId.Equals(tech_id)
                && a.AgencyId.Equals(agency_id)
                && (a.RequestStatus!.Equals("WARNING")
                || a.RequestStatus!.Equals("PREPARING")
                || a.RequestStatus!.Equals("RESOLVING")
                || a.RequestStatus!.Equals("RESOLVED")
                || a.RequestStatus!.Equals("COMPLETED"))).Select(a => new RequestResponse
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
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
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
                    description = a.RequestDesciption,
                    admin_id = a.AdminId,
                    contract = new ContractViewResponse
                    {
                        id = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.ContractName).FirstOrDefault(),
                    },
                    reject_reason = a.ReasonReject,
                    technicican = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
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
                if (value.search.ToLower().Contains("admin"))
                {
                    total = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(tech_id)
                    && a.AgencyId.Equals(agency_id)
                    && (a.RequestStatus!.Contains(value.status!)
                    && a.AdminId != null)
                    && (a.RequestStatus!.Equals("WARNING")
                    || a.RequestStatus!.Equals("PREPARING")
                    || a.RequestStatus!.Equals("RESOLVING")
                    || a.RequestStatus!.Equals("RESOLVED")
                    || a.RequestStatus!.Equals("COMPLETED"))).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(tech_id)
                    && a.AgencyId.Equals(agency_id)
                    && (a.RequestStatus!.Contains(value.status!)
                    && a.AdminId != null)
                    && (a.RequestStatus!.Equals("WARNING")
                    || a.RequestStatus!.Equals("PREPARING")
                    || a.RequestStatus!.Equals("RESOLVING")
                    || a.RequestStatus!.Equals("RESOLVED")
                    || a.RequestStatus!.Equals("COMPLETED"))).Select(a => new RequestResponse
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
                            phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
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
                        admin_id = a.AdminId,
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
                    var agency_name = await _context.Agencies.Where(a => a.AgencyName!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                    var customer_name = await _context.Customers.Where(a => a.Name!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                    var contract_name = await _context.Contracts.Where(a => a.ContractName!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                    var service_name = await _context.Services.Where(a => a.ServiceName!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                    total = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(tech_id)
                    && a.AgencyId.Equals(agency_id)
                    && (a.RequestStatus!.Contains(value.status!)
                    && (a.RequestName!.Contains(value.search!)
                    || a.Code!.Contains(value.search!)
                    || a.RequestDesciption!.Contains(value.search!)
                    || a.AgencyId!.Equals(agency_name)
                    || a.CustomerId!.Equals(customer_name)
                    || a.ContractId!.Equals(contract_name)
                    || a.ServiceId!.Equals(service_name))
                    && (a.RequestStatus!.Equals("WARNING")
                    || a.RequestStatus!.Equals("PREPARING")
                    || a.RequestStatus!.Equals("RESOLVING")
                    || a.RequestStatus!.Equals("RESOLVED")
                    || a.RequestStatus!.Equals("COMPLETED")))).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                    && a.CurrentTechnicianId.Equals(tech_id)
                    && a.AgencyId.Equals(agency_id)
                    && (a.RequestName!.Contains(value.search!)
                    || a.Code!.Contains(value.search)
                    || a.RequestDesciption!.Contains(value.search!)
                    || a.AgencyId!.Equals(agency_name)
                    || a.CustomerId!.Equals(customer_name)
                    || a.ContractId!.Equals(contract_name)
                    || a.ServiceId!.Equals(service_name))
                    && (a.RequestStatus!.Contains(value.status!))
                    && (a.RequestStatus!.Equals("WARNING")
                    || a.RequestStatus!.Equals("PREPARING")
                    || a.RequestStatus!.Equals("RESOLVING")
                    || a.RequestStatus!.Equals("RESOLVED")
                    || a.RequestStatus!.Equals("COMPLETED"))).Select(a => new RequestResponse
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
                            phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
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
            
            var list = new List<DevicesOfRequestResponse>();
            var message = "blank";
            var status = 500;
            if (model.ticket.Count <= 0)
            {
                message = "Device must not be empty!";
                status = 400;
            }
            else
            {
                technician!.IsBusy = false;
                request!.UpdateDate = DateTime.UtcNow.AddHours(7);
                request!.RequestStatus = ProcessStatus.RESOLVED.ToString();
                request.EndTime = DateTime.UtcNow.AddHours(7);
                message = "Successfully";
                status = 200;
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

                    if (item.img!.Count > 0)
                    {
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
                                ObjectName = ObjectName.TI.ToString(),
                            };
                            await _context.Images.AddAsync(imgTicket);
                        }
                    }
                    await _context.Tickets.AddAsync(ticket);
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
                        img = _context.Images.Where(a => a.CurrentObject_Id.Equals(ticket.Id) && a.ObjectName!.Equals(ObjectName.TI.ToString())).Select(x => x.Link).ToList()!,
                    });
                }
            }
            return new ResponseModel<DevicesOfRequestResponse>(list)
            {
                Message = message,
                Status = status,
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
            request!.UpdateDate = DateTime.UtcNow.AddHours(7);
            request!.RequestStatus = ProcessStatus.RESOLVED.ToString();
            var list = new List<DevicesOfRequestResponse>();
            var message = "blank";
            var status = 500;
            if (model.ticket.Count <= 0)
            {
                message = "Device must not be empty!";
                status = 400;
            }
            else
            {
                message = "Successfully";
                status = 201;
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

                    if (item.img!.Count > 0)
                    {
                        foreach (var item1 in item.img!)
                        {
                            var img_id = Guid.NewGuid();
                            while (true)
                            {
                                var img_dup = await _context.Images.Where(x => x.Id.Equals(img_id) && x.ObjectName.Equals(ObjectName.TI.ToString())).FirstOrDefaultAsync();
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
                                ObjectName = ObjectName.TI.ToString(),
                            };
                            await _context.Images.AddAsync(imgTicket);

                        }
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
            }

            return new ObjectModelResponse(list!)
            {
                Status = status,
                Message = message,
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
                Breach = 0,
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
            var message = "blank";
            var status = 500;
            var tech_mail = await _context.Technicians.Where(x => x.Email!.Equals(technician.Email) && x.IsDelete == false).FirstOrDefaultAsync();
            var tech_phone = await _context.Technicians.Where(x => x.Telephone!.Equals(technician.Telephone) && x.IsDelete == false).FirstOrDefaultAsync();
            if (tech_mail != null)
            {
                status = 400;
                message = "Mail is already exists!";
            }
            else if (tech_phone != null)
            {
                status = 400;
                message = "Phone is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 200;
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
            }


            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
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
                        item.UpdateDate = DateTime.UtcNow.AddHours(7);
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
            var maintain_techs = await _context.MaintenanceSchedules.Where(a => a.TechnicianId.Equals(id) && a.IsDelete == false).ToListAsync();
            foreach (var item in maintain_techs)
            {
                item.TechnicianId = null;
            }
            var account = await _context.Accounts.Where(a => a.IsDelete == false && a.Id.Equals(technician.AccountId)).FirstOrDefaultAsync();
            if (account != null)
            {
                account.IsAssign = false;
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
        public async Task<ObjectModelResponse> IsBusyTechnician(Guid id, IsBusyRequest model)
        {
            var technician = await _context.Technicians.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            var message = "blank";
            var status = 500;
            var data = new TechnicianUpdateResponse();
            var request = await _context.Requests.Where(a => a.CurrentTechnicianId.Equals(id) && a.RequestStatus!.Equals("RESOLVING")).FirstOrDefaultAsync();
            var maintain = await _context.MaintenanceSchedules.Where(a => a.TechnicianId.Equals(id) && a.Status!.Equals("MAINTAINING")).FirstOrDefaultAsync();
            if (request != null || maintain != null)
            {
                message = "You have a request or maintenance schedule that needs to solve";
                status = 400;
            }
            else
            {
                message = "Successfully";
                status = 200;
                technician!.IsBusy = model.is_busy;
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
    
        public async Task<ObjectModelResponse> RejectRequest(Guid id, Guid tech_id)
        {
            var request = await _context.Requests.Where(x => x.Id.Equals(id) && x.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(x => x.Id.Equals(request!.CurrentTechnicianId)).FirstOrDefaultAsync();
            var message = "blank";
            var status = 500;
            var data = new ResolvingRequestResponse();
            if (request!.CurrentTechnicianId.Equals(tech_id))
            {
                message = "Successfully";
                status = 200;
                technician!.IsBusy = true;
                var currentTechnician = await _context.Technicians.Where(x => x.Id.Equals(request!.CurrentTechnicianId)).Select(a => new TechnicianOfRequestResponse
                {
                    id = a.Id,
                    code = a.Code,
                    technician_name = a.TechnicianName,
                }).ToListAsync();
                var agency = await _context.Agencies.Where(a => a.Id.Equals(request!.AgencyId)).FirstOrDefaultAsync();
                var area = await _context.Areas.Where(a => a.Id.Equals(agency!.AreaId)).FirstOrDefaultAsync();
                var service = await _context.Services.Where(a => a.Id.Equals(request!.ServiceId)).FirstOrDefaultAsync();
                var total = await _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
                   && a.Technician!.AreaId.Equals(area!.Id)
                   && a.Technician.IsBusy == false
                   && a.Technician.IsDelete == false).ToListAsync();
                var technicians = new List<TechnicianOfRequestResponse>();
                DateTime date = DateTime.UtcNow.AddHours(7);
                if (total.Count > 0)
                {
                    foreach (var item in total)
                    {
                        date = date.AddDays((-date.Day) + 1).Date;
                        var requests = await _context.Requests.Where(a => a.IsDelete == false
                        && a.CurrentTechnicianId.Equals(item.TechnicianId)
                        && a.RequestStatus!.Equals("COMPLETED")
                        && a.CreateDate!.Value.Date >= date
                        && a.CreateDate!.Value.Date <= DateTime.UtcNow.AddHours(7)).ToListAsync();
                        var count = requests.Count;
                        technicians.Add(new TechnicianOfRequestResponse
                        {
                            id = item.TechnicianId,
                            code = _context.Technicians.Where(a => a.IsDelete == false && a.Id.Equals(item.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                            technician_name = _context.Technicians.Where(a => a.IsDelete == false && a.Id.Equals(item.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                            number_of_requests = count,
                            area = area!.AreaName,
                            skills = _context.Skills.Where(a => a.TechnicianId.Equals(item!.TechnicianId)).Select(a => a.Service.ServiceName).ToList()!,
                        });
                    }
                }
                else
                {
                    total = await _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
                                    && a.Technician.IsBusy == false
                                    && a.Technician.IsDelete == false).ToListAsync();
                    foreach (var item in total)
                    {
                        date = date.AddDays((-date.Day) + 1).Date;
                        var requests = await _context.Requests.Where(a => a.IsDelete == false
                        && a.CurrentTechnicianId.Equals(item.TechnicianId)
                        && a.RequestStatus.Equals("COMPLETED")
                        && a.CreateDate!.Value.Date >= date
                        && a.CreateDate!.Value.Date <= DateTime.UtcNow.AddHours(7)).ToListAsync();
                        var count = requests.Count;
                        technicians.Add(new TechnicianOfRequestResponse
                        {
                            id = item.TechnicianId,
                            code = _context.Technicians.Where(a => a.IsDelete == false && a.Id.Equals(item.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                            technician_name = _context.Technicians.Where(a => a.IsDelete == false && a.Id.Equals(item.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                            number_of_requests = count,
                            area = area!.AreaName,
                            skills = _context.Skills.Where(a => a.TechnicianId.Equals(item!.TechnicianId)).Select(a => a.Service.ServiceName).ToList()!,
                        });
                    }
                }
                technicians.OrderBy(a => a.number_of_requests).Except(currentTechnician).ToList();
                if(technicians.Count > 0)
                {
                    request!.UpdateDate = DateTime.UtcNow.AddHours(7);
                    request!.StartTime = DateTime.UtcNow.AddHours(7);
                    request!.RequestStatus = ProcessStatus.PREPARING.ToString();
                    request!.CurrentTechnicianId = technicians.FirstOrDefault()!.id;
                }
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    data = new ResolvingRequestResponse
                    {
                        id = id,
                        code = request.Code,
                        status = request.RequestStatus,
                        name = request.RequestName,
                        technician = _context.Technicians.Where(a => a.IsDelete == false && a.Id.Equals(request.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    };
                }
            }
            else
            {
                message = "The technician does not own the request";
                status = 400;
            }


            return new ObjectModelResponse(data)
            {
                Status = status,
                Message = message,
                Type = "Request"
            };
        }
        public async Task<ObjectModelResponse> ResolvingRequest(Guid id, Guid tech_id)
        {
            var request = await _context.Requests.Where(x => x.Id.Equals(id) && x.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(x => x.Id.Equals(request!.CurrentTechnicianId)).FirstOrDefaultAsync();
            var message = "blank";
            var status = 500;
            var data = new ResolvingRequestResponse();
            var request_resolving = await _context.Requests.Where(a => a.CurrentTechnicianId.Equals(tech_id) && (a.RequestStatus!.Equals("RESOLVING") || a.RequestStatus!.Equals("EDITING"))).FirstOrDefaultAsync();
            var maintain = await _context.MaintenanceSchedules.Where(a => a.TechnicianId.Equals(tech_id) && a.Status!.Equals("MAINTAINING") && a.IsDelete == false).FirstOrDefaultAsync();
            if (request_resolving != null || maintain != null)
            {
                message = "You have a request or maintenance schedule that needs to solve";
                status = 400;
            }
            else if (request!.CurrentTechnicianId.Equals(tech_id))
            {
                message = "Successfully";
                status = 200;
                technician!.IsBusy = true;
                request!.RequestStatus = ProcessStatus.RESOLVING.ToString();
                request!.UpdateDate = DateTime.UtcNow.AddHours(7);
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    data = new ResolvingRequestResponse
                    {
                        id = id,
                        code = request.Code,
                        status = request.RequestStatus,
                        name = request.RequestName,
                        technician = _context.Technicians.Where(a => a.IsDelete == false && a.Id.Equals(request.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    };
                }
            }
            else
            {
                message = "Error";
                status = 400;
            }


            return new ObjectModelResponse(data)
            {
                Status = status,
                Message = message,
                Type = "Request"
            };
        }
        public async Task<ResponseModel<Technician>> ResetBreachTechnician()
        {
            var technicians = await _context.Technicians.Where(a=>a.IsDelete == false).ToListAsync();
            foreach (var item in technicians)
            {
                item!.Breach = 0;
            }
            await _context.SaveChangesAsync();
            return new ResponseModel<Technician>(technicians)
            {
                Total = technicians.Count,
                Type = "Technician"
            };
        }
        public async Task<ObjectModelResponse> UpdateTechnician(Guid id, TechnicianUpdateRequest model)
        {
            var technician = await _context.Technicians.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
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
            var message = "blank";
            var status = 500;
            var tech_mail = await _context.Technicians.Where(x => x.Email!.Equals(model.email) && x.IsDelete == false).FirstOrDefaultAsync();
            var tech_phone = await _context.Technicians.Where(x => x.Telephone!.Equals(model.telephone) && x.IsDelete == false).FirstOrDefaultAsync();
            if (tech_mail != null && technician!.Email != model.email)
            {
                status = 400;
                message = "Mail is already exists!";
            }
            else if (tech_phone != null && technician!.Telephone != model.telephone)
            {
                status = 400;
                message = "Phone is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 200;
                technician!.UpdateDate = DateTime.UtcNow.AddHours(7);
                technician!.TechnicianName = model.technician_name;
                technician!.Address = model.address;
                technician!.Telephone = model.telephone;
                technician!.AreaId = model.area_id;
                technician!.Gender = model.gender;
                technician!.Email = model.email;
                technician!.Breach = model.breach;

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
            }
            return new ObjectModelResponse(data)
            {
                Status = status,
                Message = message,
                Type = "Technician"
            };
        }


    }
}
