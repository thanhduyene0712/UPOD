﻿using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Enum;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{
    public interface IRequestService
    {
        Task<ResponseModel<RequestResponse>> GetListRequests(PaginationRequest model, FilterStatusRequest value);
        Task<ObjectModelResponse> GetDetailsRequest(Guid id);
        Task<ObjectModelResponse> CreateRequest(RequestRequest model);
        Task<ObjectModelResponse> UpdateRequest(Guid id, RequestUpdateRequest model);
        Task<ObjectModelResponse> DisableRequest(Guid id);
        Task<ResponseModel<TechnicianRequestResponse>> GetTechnicianRequest(PaginationRequest model, Guid id);
        Task<ObjectModelResponse> MappingTechnicianRequest(Guid request_id, Guid technician_id);
        Task<ResponseModel<DeviceResponse>> GetDeviceRequest(PaginationRequest model, Guid id);
        Task<ObjectModelResponse> CreateRequestByAdmin(RequestAdminRequest model);
        Task<ObjectModelResponse> RejectRequest(Guid id, RejectRequest value);
        Task<ObjectModelResponse> ReOpenRequest(Guid id);
        Task<ObjectModelResponse> CancelRequest(Guid id);
        Task<ObjectModelResponse> AutoFillRequestAdmin(Guid id);
        Task<ResponseModel<RequestResponse>> GetListRequestsOfAgency(PaginationRequest model, Guid id, FilterStatusRequest value);
    }
    public class RequestServices : IRequestService
    {

        private readonly Database_UPODContext _context;
        public RequestServices(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ObjectModelResponse> AutoFillRequestAdmin(Guid id)
        {
            var reportSchedule = await _context.MaintenanceReportServices.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            var maintenance_report = await _context.MaintenanceReports.Where(a => a.Id.Equals(reportSchedule!.MaintenanceReportId)).FirstOrDefaultAsync();
            var agency = await _context.Agencies.Where(a => a.Id.Equals(reportSchedule!.MaintenanceReport!.AgencyId)).FirstOrDefaultAsync();
            var customer = await _context.Customers.Where(a => a.Id.Equals(agency!.CustomerId)).FirstOrDefaultAsync();
            var service = await _context.Services.Where(a => a.Id.Equals(reportSchedule!.ServiceId)).FirstOrDefaultAsync();
            var request = new AutoFillRequestResponse
            {
                report_service_id = reportSchedule!.Id,
                customer = new CustomerViewResponse
                {
                    id = customer!.Id,
                    code = customer!.Code,
                    cus_name = customer!.Name,
                    mail = customer!.Mail,
                    address = customer!.Address,
                    phone = customer!.Phone,
                    description = customer.Description,
                },
                agency = new AgencyViewResponse
                {
                    id = agency!.Id,
                    code = agency!.Code,
                    address = agency!.Address,
                    phone = agency!.Telephone,
                    agency_name = agency.AgencyName
                },
                service = new ServiceNotInContractViewResponse
                {
                    id = service!.Id,
                    code = service!.Code,
                    service_name = service!.ServiceName
                },
                request_description = reportSchedule!.Description,
            };
            return new ObjectModelResponse(request!)
            {
                Type = "Request"
            };
        }
        public async Task<ResponseModel<RequestResponse>> GetListRequestsOfAgency(PaginationRequest model, Guid id, FilterStatusRequest value)
        {
            var total = await _context.Requests.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).ToListAsync();
            var requests = new List<RequestResponse>();
            if (value.search == null && value.status == null)
            {
                total = await _context.Requests.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).ToListAsync();
                requests = await _context.Requests.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).Select(a => new RequestResponse
                {
                    id = a.Id,
                    code = a.Code,
                    request_name = a.RequestName,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                        cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    admin_id = a.AdminId,
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
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
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
                                        && a.AgencyId.Equals(id)
                                        && (a.RequestStatus!.Contains(value.status!.Trim())
                                        && (a.AdminId != null))).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                    && a.AgencyId.Equals(id)
                    && (a.RequestStatus!.Contains(value.status!.Trim())
                    && (a.AdminId != null))).Select(a => new RequestResponse
                    {
                        id = a.Id,
                        code = a.Code,
                        request_name = a.RequestName,
                        customer = new CustomerViewResponse
                        {
                            id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                            code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                            address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                            mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
                            phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                            cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                            description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                        },
                        admin_id = a.AdminId,
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
                        priority = a.Priority,
                        request_status = a.RequestStatus,
                        create_date = a.CreateDate,
                        update_date = a.UpdateDate,
                        technicican = new TechnicianViewResponse
                        {
                            id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                            code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                            tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                            email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                            phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                        },
                    }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
                }
                else
                {
                    var customer_name = await _context.Customers.Where(a => a.Name!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    var contract_name = await _context.Contracts.Where(a => a.ContractName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    var service_name = await _context.Services.Where(a => a.ServiceName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    total = await _context.Requests.Where(a => a.IsDelete == false
                    && a.AgencyId.Equals(id)
                    && (a.RequestStatus!.Contains(value.status!.Trim())
                    && (a.RequestName!.Contains(value.search!.Trim())
                    || a.Code!.Contains(value.search!.Trim())
                    || a.CustomerId!.Equals(customer_name)
                    || a.ContractId!.Equals(contract_name)
                    || a.ServiceId!.Equals(service_name)))).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                    && a.AgencyId.Equals(id)
                    && (a.RequestName!.Contains(value.search!.Trim())
                    || a.Code!.Contains(value.search.Trim())
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
                            address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                            mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
                            phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                            cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                            description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                        },
                        admin_id = a.AdminId,
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
                        priority = a.Priority,
                        request_status = a.RequestStatus,
                        create_date = a.CreateDate,
                        update_date = a.UpdateDate,
                        technicican = new TechnicianViewResponse
                        {
                            id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                            code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                            tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                            email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                            phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                        },
                    }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
                }

            }
            return new ResponseModel<RequestResponse>(requests)
            {
                Total = total.Count,
                Type = "Request"
            };
        }
        public async Task<ResponseModel<RequestResponse>> GetListRequests(PaginationRequest model, FilterStatusRequest value)
        {
            var total = await _context.Requests.Where(a => a.IsDelete == false).ToListAsync();
            var requests = new List<RequestResponse>();

            if (value.search == null && value.status == null)
            {
                total = await _context.Requests.Where(a => a.IsDelete == false).ToListAsync();
                requests = await _context.Requests.Where(a => a.IsDelete == false).Select(a => new RequestResponse
                {
                    id = a.Id,
                    code = a.Code,
                    request_name = a.RequestName,
                    admin_id = a.AdminId,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                        cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
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
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
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
                    && (a.RequestStatus!.Contains(value.status!.Trim())
                    && (a.AdminId != null))).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                    && (a.RequestStatus!.Contains(value.status!.Trim())
                    && (a.AdminId != null))).Select(a => new RequestResponse
                    {
                        id = a.Id,
                        code = a.Code,
                        request_name = a.RequestName,
                        customer = new CustomerViewResponse
                        {
                            id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                            code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                            address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                            mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
                            phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                            cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                            description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                        },
                        admin_id = a.AdminId,
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
                        priority = a.Priority,
                        request_status = a.RequestStatus,
                        create_date = a.CreateDate,
                        update_date = a.UpdateDate,
                        technicican = new TechnicianViewResponse
                        {
                            id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                            code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                            tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                            email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                            phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                        },
                    }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
                }
                else
                {
                    var agency_name = await _context.Agencies.Where(a => a.AgencyName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    var customer_name = await _context.Customers.Where(a => a.Name!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    var contract_name = await _context.Contracts.Where(a => a.ContractName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    var service_name = await _context.Services.Where(a => a.ServiceName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                    total = await _context.Requests.Where(a => a.IsDelete == false
                    && (a.RequestStatus!.Contains(value.status!.Trim())
                    && (a.RequestName!.Contains(value.search!.Trim())
                    || a.Code!.Contains(value.search!.Trim())
                    || a.AgencyId!.Equals(agency_name)
                    || a.CustomerId!.Equals(customer_name)
                    || a.ContractId!.Equals(contract_name)
                    || a.ServiceId!.Equals(service_name)))).ToListAsync();
                    requests = await _context.Requests.Where(a => a.IsDelete == false
                    && (a.RequestName!.Contains(value.search!.Trim())
                    || a.Code!.Contains(value.search.Trim())
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
                            address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                            mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
                            phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                            cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                            description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                        },
                        admin_id = a.AdminId,
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
                        priority = a.Priority,
                        request_status = a.RequestStatus,
                        create_date = a.CreateDate,
                        update_date = a.UpdateDate,
                        technicican = new TechnicianViewResponse
                        {
                            id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                            code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                            tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                            email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                            phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                        },
                    }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
                }

            }

            return new ResponseModel<RequestResponse>(requests)
            {
                Total = total.Count,
                Type = "Requests"
            };
        }

        public async Task<ResponseModel<DeviceResponse>> GetDeviceRequest(PaginationRequest model, Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var agency = await _context.Agencies.Where(a => a.Id.Equals(request!.AgencyId) && a.IsDelete == false).FirstOrDefaultAsync();
            var total = await _context.Devices.Where(a => a.AgencyId.Equals(agency!.Id) && a.IsDelete == false).ToListAsync();
            var device = await _context.Devices.Where(a => a.AgencyId.Equals(agency!.Id) && a.IsDelete == false).Select(a => new DeviceResponse
            {
                id = a.Id,
                code = a.Code,
                agency = new AgencyViewResponse
                {
                    id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                    agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                    address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                },
                devicetype = new DeviceTypeViewResponse
                {
                    id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                    service_id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                    device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                    code = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                },
                device_name = a.DeviceName,
                guaranty_start_date = a.GuarantyStartDate,
                guaranty_end_date = a.GuarantyEndDate,
                ip = a.Ip,
                port = a.Port,
                device_account = a.DeviceAccount,
                device_password = a.DevicePassword,
                setting_date = a.SettingDate,
                other = a.Other,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,


            }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<DeviceResponse>(device)
            {
                Total = total.Count,
                Type = "Devices"
            };
        }
        public async Task<ResponseModel<TechnicianRequestResponse>> GetTechnicianRequest(PaginationRequest model, Guid id)
        {

            var request = await _context.Requests.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            var agency = await _context.Agencies.Where(a => a.Id.Equals(request!.AgencyId)).FirstOrDefaultAsync();
            var area = await _context.Areas.Where(a => a.Id.Equals(agency!.AreaId)).FirstOrDefaultAsync();
            var service = await _context.Services.Where(a => a.Id.Equals(request!.ServiceId)).FirstOrDefaultAsync();
            var technicians = new List<TechnicianRequestResponse>();
            var total = await _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
            && a.Technician!.AreaId.Equals(area!.Id)
            && a.Technician.IsBusy == false
            && a.Technician.IsDelete == false
            && a.TechnicianId.Equals(agency!.TechnicianId)).Include(a => a.Technician).ToListAsync();

            var technicianDefault = await _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
            && a.Technician!.AreaId.Equals(area!.Id)
            && a.Technician.IsBusy == false
            && a.Technician.IsDelete == false
            && a.TechnicianId.Equals(agency!.TechnicianId)).Include(a => a.Technician).FirstOrDefaultAsync();
            if (technicianDefault != null)
            {
                technicians.Add(new TechnicianRequestResponse
                {
                    id = technicianDefault.TechnicianId,
                    code = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                    area = new AreaViewResponse
                    {
                        id = _context.Areas.Where(x => x.Id.Equals(technicianDefault.Technician!.AreaId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Areas.Where(x => x.Id.Equals(technicianDefault.Technician!.AreaId)).Select(x => x.Code).FirstOrDefault(),
                        area_name = _context.Areas.Where(x => x.Id.Equals(technicianDefault.Technician!.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                        description = _context.Areas.Where(x => x.Id.Equals(technicianDefault.Technician!.AreaId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    technician_name = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    account = new AccountViewResponse
                    {
                        id = _context.Accounts.Where(x => x.Id.Equals(technicianDefault.Technician!.AccountId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Accounts.Where(x => x.Id.Equals(technicianDefault.Technician!.AccountId)).Select(x => x.Code).FirstOrDefault(),
                        role_name = _context.Accounts.Where(x => x.Id.Equals(technicianDefault.Technician!.AccountId)).Select(x => x.Role!.RoleName).FirstOrDefault(),
                        username = _context.Accounts.Where(x => x.Id.Equals(technicianDefault.Technician!.AccountId)).Select(x => x.Username).FirstOrDefault(),
                        password = _context.Accounts.Where(x => x.Id.Equals(technicianDefault.Technician!.AccountId)).Select(x => x.Password).FirstOrDefault(),
                    },
                    telephone = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                    email = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.Email).FirstOrDefault(),
                    gender = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.Gender).FirstOrDefault(),
                    address = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.Address).FirstOrDefault(),
                    is_busy = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.IsBusy).FirstOrDefault(),
                    is_delete = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.IsDelete).FirstOrDefault(),
                    create_date = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.CreateDate).FirstOrDefault(),
                    update_date = _context.Technicians.Where(a => a.Id.Equals(technicianDefault.TechnicianId)).Select(a => a.UpdateDate).FirstOrDefault(),

                });
            }
            else
            {
                total = await _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
                && a.Technician!.AreaId.Equals(area!.Id)
                && a.Technician.IsBusy == false
                && a.Technician.IsDelete == false).ToListAsync();
                if (total!.Count <= 0)
                {
                    total = await _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
                && a.Technician!.IsBusy == false
                && a.Technician.IsDelete == false).ToListAsync();
                    technicians = await _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
                && a.Technician!.IsBusy == false
                && a.Technician.IsDelete == false).Select(a => new TechnicianRequestResponse
                {
                    id = a.TechnicianId,
                    code = a.Technician!.Code,
                    area = new AreaViewResponse
                    {
                        id = _context.Areas.Where(x => x.Id.Equals(a.Technician.AreaId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Areas.Where(x => x.Id.Equals(a.Technician.AreaId)).Select(x => x.Code).FirstOrDefault(),
                        area_name = _context.Areas.Where(x => x.Id.Equals(a.Technician.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                        description = _context.Areas.Where(x => x.Id.Equals(a.Technician.AreaId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    technician_name = a.Technician.TechnicianName,
                    account = new AccountViewResponse
                    {
                        id = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Code).FirstOrDefault(),
                        role_name = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Role!.RoleName).FirstOrDefault(),
                        username = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Username).FirstOrDefault(),
                        password = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Password).FirstOrDefault(),
                    },
                    telephone = a.Technician.Telephone,
                    email = a.Technician.Email,
                    gender = a.Technician.Gender,
                    address = a.Technician.Address,
                    is_busy = a.Technician.IsBusy,
                    is_delete = a.Technician.IsDelete,
                    create_date = a.Technician.CreateDate,
                    update_date = a.Technician.UpdateDate,

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
                }
                else
                {
                    technicians = await _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
                && a.Technician!.AreaId.Equals(area!.Id)
                && a.Technician.IsBusy == false
                && a.Technician.IsDelete == false).Select(a => new TechnicianRequestResponse
                {
                    id = a.TechnicianId,
                    code = a.Technician!.Code,
                    area = new AreaViewResponse
                    {
                        id = _context.Areas.Where(x => x.Id.Equals(a.Technician.AreaId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Areas.Where(x => x.Id.Equals(a.Technician.AreaId)).Select(x => x.Code).FirstOrDefault(),
                        area_name = _context.Areas.Where(x => x.Id.Equals(a.Technician.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                        description = _context.Areas.Where(x => x.Id.Equals(a.Technician.AreaId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    technician_name = a.Technician.TechnicianName,
                    account = new AccountViewResponse
                    {
                        id = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Code).FirstOrDefault(),
                        role_name = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Role!.RoleName).FirstOrDefault(),
                        username = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Username).FirstOrDefault(),
                        password = _context.Accounts.Where(x => x.Id.Equals(a.Technician.AccountId)).Select(x => x.Password).FirstOrDefault(),
                    },
                    telephone = a.Technician.Telephone,
                    email = a.Technician.Email,
                    gender = a.Technician.Gender,
                    address = a.Technician.Address,
                    is_busy = a.Technician.IsBusy,
                    is_delete = a.Technician.IsDelete,
                    create_date = a.Technician.CreateDate,
                    update_date = a.Technician.UpdateDate,

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
                }


            }
            return new ResponseModel<TechnicianRequestResponse>(technicians)
            {
                Total = total.Count,
                Type = "Technicians"
            };
        }
        public async Task<ObjectModelResponse> GetDetailsRequest(Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var request_details = new RequestDetailsResponse();
            if (request!.AdminId == null)
            {
                request_details = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new RequestDetailsResponse
                {
                    id = a.Id,
                    code = a.Code,
                    request_name = a.RequestName,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                        cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
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
                    start_time = a.StartTime,
                    end_time = a.EndTime,
                    create_by = new CreateByViewModel
                    {
                        id = a.CustomerId,
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        role = "Customer",
                    },
                    technicican = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                    },
                    contract = new ContractViewResponse
                    {
                        id = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.ContractName).FirstOrDefault(),
                    }
                }).FirstOrDefaultAsync();
            }
            else
            {
                request_details = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new RequestDetailsResponse
                {
                    id = a.Id,
                    code = a.Code,
                    request_name = a.RequestName,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Phone).FirstOrDefault(),
                        cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
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
                    create_by = new CreateByViewModel
                    {
                        id = a.AdminId,
                        code = _context.Admins.Where(x => x.Id.Equals(a.AdminId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Admins.Where(x => x.Id.Equals(a.AdminId)).Select(a => a.Name).FirstOrDefault(),
                        role = "Admin",
                    },
                    technicican = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Email).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(a.CurrentTechnicianId)).Select(a => a.Telephone).FirstOrDefault(),
                    },
                    contract = new ContractViewResponse
                    {
                        id = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Contracts.Where(x => x.Id.Equals(a.ContractId)).Select(a => a.ContractName).FirstOrDefault(),
                    }
                }).FirstOrDefaultAsync();
            }




            return new ObjectModelResponse(request_details!)
            {
                Type = "Request"
            };

        }
        public async Task<ObjectModelResponse> MappingTechnicianRequest(Guid request_id, Guid technician_id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(request_id) && a.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(a => a.Id.Equals(technician_id) && a.IsDelete == false).FirstOrDefaultAsync();
            technician!.IsBusy = true;
            request!.CurrentTechnicianId = technician_id;
            request.RequestStatus = ProcessStatus.PREPARING.ToString();
            _context.Requests.Update(request);
            _context.Technicians.Update(technician);
            var data = new MappingTechnicianResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new MappingTechnicianResponse
                {
                    id = request_id,
                    technician_id = technician_id,
                    request_status = request.RequestStatus,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Request"
            };
        }
        public async Task<ObjectModelResponse> CreateRequest(RequestRequest model)
        {
            var request_id = Guid.NewGuid();
            while (true)
            {
                var request_dup = await _context.Requests.Where(x => x.Id.Equals(request_id)).FirstOrDefaultAsync();
                if (request_dup == null)
                {
                    break;
                }
                else
                {
                    request_id = Guid.NewGuid();
                }
            }
            var num = await GetLastCode();
            var code = CodeHelper.GeneratorCode("RE", num + 1);
            while (true)
            {
                var code_dup = await _context.Requests.Where(a => a.Code.Equals(code)).FirstOrDefaultAsync();
                if (code_dup == null)
                {
                    break;
                }
                else
                {
                    code = "RE-" + num++.ToString(); 
                }
            }
            var customer_id = await _context.Agencies.Where(a => a.Id.Equals(model.agency_id)).Select(a => a.CustomerId).FirstOrDefaultAsync();
            var contracts = await _context.Contracts.Where(a => a.CustomerId.Equals(customer_id)).ToListAsync();
            Guid? contract_id = null;
            foreach (var item in contracts)
            {
                var contract_services = await _context.ContractServices.Where(a => a.ContractId.Equals(item.Id)).ToListAsync();
                foreach (var item1 in contract_services)
                {
                    if (item1.ServiceId.Equals(model.service_id))
                    {
                        contract_id = item1.ContractId;
                    }
                }
            }
            var request = new Request
            {
                Id = request_id,
                Code = code,
                RequestName = model.request_name,
                CustomerId = customer_id,
                ServiceId = model.service_id,
                AgencyId = model.agency_id,
                RequestDesciption = model.request_description,
                RequestStatus = ProcessStatus.PENDING.ToString(),
                ReasonReject = null,
                Priority = model.priority,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7),
                IsDelete = false,
                Feedback = null,
                Rating = 0,
                CurrentTechnicianId = null,
                StartTime = null,
                EndTime = null,
                AdminId = null,
                ContractId = contract_id
            };
            var data = new RequestCreateResponse();

            await _context.Requests.AddAsync(request);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new RequestCreateResponse
                {
                    id = request.Id,
                    code = request.Code,
                    request_name = request.RequestName,
                    request_description = request.RequestDesciption,
                    priority = request.Priority,
                    phone = _context.Agencies.Where(x => x.Id.Equals(request.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    agency_name = _context.Agencies.Where(x => x.Id.Equals(request.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                    service_name = _context.Services.Where(x => x.Id.Equals(request.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                    customer_name = _context.Customers.Where(x => x.Id.Equals(request.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                };
            }
            return new ObjectModelResponse(data)
            {
                Type = "Request"
            };
        }
        public async Task<ObjectModelResponse> CreateRequestByAdmin(RequestAdminRequest model)
        {
            var request_id = Guid.NewGuid();
            while (true)
            {
                var request_dup = await _context.Requests.Where(x => x.Id.Equals(request_id)).FirstOrDefaultAsync();
                if (request_dup == null)
                {
                    break;
                }
                else
                {
                    request_id = Guid.NewGuid();
                }
            }
            var num = await GetLastCode();
            var code = CodeHelper.GeneratorCode("RE", num + 1);
            while (true)
            {
                var code_dup = await _context.Requests.Where(a => a.Code.Equals(code)).FirstOrDefaultAsync();
                if (code_dup == null)
                {
                    break;
                }
                else
                {
                    code = "RE-" + num++.ToString();
                }
            }
            var contracts = await _context.Contracts.Where(a => a.CustomerId.Equals(model.customer_id)).ToListAsync();
            Guid? contract_id = null;
            foreach (var item in contracts)
            {
                var contract_services = await _context.ContractServices.Where(a => a.ContractId.Equals(item.Id)).ToListAsync();
                foreach (var item1 in contract_services)
                {
                    if (item1.ServiceId.Equals(model.service_id))
                    {
                        contract_id = item1.ContractId;
                    }
                }
            }
            var request = new Request
            {
                Id = request_id,
                Code = code,
                RequestName = model.request_name,
                CustomerId = model.customer_id,
                ServiceId = model.service_id,
                AgencyId = model.agency_id,
                RequestDesciption = model.request_description,
                RequestStatus = ProcessStatus.PREPARING.ToString(),
                ReasonReject = null,
                Priority = model.priority,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7),
                IsDelete = false,
                Feedback = null,
                Rating = 0,
                CurrentTechnicianId = model.technician_id,
                StartTime = null,
                EndTime = null,
                AdminId = model.admin_id,
                ContractId = contract_id
            };
            var report_service = await _context.MaintenanceReportServices.Where(a => a.Id.Equals(model.report_service_id)).FirstOrDefaultAsync();
            report_service!.Created = true;
            report_service!.RequestId = request_id!;
            var data = new RequestCreateResponse();

            await _context.Requests.AddAsync(request);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new RequestCreateResponse
                {
                    id = request.Id,
                    code = request.Code,
                    request_name = request.RequestName,
                    request_description = request.RequestDesciption,
                    priority = request.Priority,
                    phone = _context.Agencies.Where(x => x.Id.Equals(request.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    agency_name = _context.Agencies.Where(x => x.Id.Equals(request.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                    customer_name = _context.Customers.Where(x => x.Id.Equals(request.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                    service_name = _context.Services.Where(x => x.Id.Equals(request.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                    technician_name = _context.Technicians.Where(x => x.Id.Equals(request.CurrentTechnicianId)).Select(x => x.TechnicianName).FirstOrDefault(),
                };


            }
            return new ObjectModelResponse(data)
            {
                Type = "Request"
            };
        }

        private async Task<int> GetLastCode()
        {
            var request = await _context.Requests.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(request!.Code!);
        }
        public async Task<ObjectModelResponse> UpdateRequest(Guid id, RequestUpdateRequest model)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id)).Select(x => new Request
            {
                Id = id,
                Code = x.Code,
                RequestName = model.request_name,
                CustomerId = _context.Agencies.Where(a => a.Id.Equals(model.agency_id)).Select(a => a.CustomerId).FirstOrDefault(),
                ServiceId = model.service_id,
                AgencyId = model.agency_id,
                RequestDesciption = model.request_description,
                RequestStatus = x.RequestStatus,
                Priority = model.priority,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.UtcNow.AddHours(7),
                IsDelete = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.IsDelete).FirstOrDefault(),
                Feedback = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.Feedback).FirstOrDefault(),
                Rating = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.Rating).FirstOrDefault(),
                CurrentTechnicianId = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.CurrentTechnicianId).FirstOrDefault(),
                StartTime = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.StartTime).FirstOrDefault(),
                EndTime = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.EndTime).FirstOrDefault(),
            }).FirstOrDefaultAsync();
            var message = "blank";
            var status = 500;
            var data = new RequestCreateResponse();
            if (request!.RequestStatus != ProcessStatus.PENDING.ToString())
            {
                status = 400;
                message = "You just update when request status is pending";
            }
            else
            {
                status = 201;
                message = "Successfully";
                _context.Requests.Update(request);
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    data = new RequestCreateResponse
                    {
                        id = request.Id,
                        code = request.Code,
                        request_name = request.RequestName,
                        request_description = request.RequestDesciption,
                        priority = request.Priority,
                        phone = _context.Agencies.Where(x => x.Id.Equals(request.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                        customer_name = _context.Customers.Where(x => x.Id.Equals(request.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(request.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(request.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                    };
                }

            }
            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
                Type = "Request"
            };
        }

        public async Task<ObjectModelResponse> ReOpenRequest(Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false && a.RequestStatus!.Equals("RESOLVED")).FirstOrDefaultAsync();
            request!.RequestStatus = ProcessStatus.EDITING.ToString();
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

        public async Task<ObjectModelResponse> CancelRequest(Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false
            && (a.RequestStatus!.Equals("PREPARING"))).FirstOrDefaultAsync();
            request!.RequestStatus = ProcessStatus.CANCELED.ToString();
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
        public async Task<ObjectModelResponse> DisableRequest(Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            request!.IsDelete = true;
            _context.Requests.Update(request);
            var data = new RequestDisableResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new RequestDisableResponse
                {
                    id = request.Id,
                    isDelete = request.IsDelete,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Request"
            };
        }
        public async Task<ObjectModelResponse> RejectRequest(Guid id, RejectRequest value)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            request!.RequestStatus = ProcessStatus.REJECTED.ToString();
            request!.ReasonReject = value.reason;

            _context.Requests.Update(request);
            var data = new RejectResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new RejectResponse
                {
                    id = request.Id,
                    code = request.Code,
                    name = request.RequestName!,
                    status = request.RequestStatus,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Request"
            };
        }
    }
}
