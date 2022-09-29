using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Enum;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{
    public interface IRequestService
    {
        Task<ResponseModel<RequestListResponse>> GetListRequests(PaginationRequest model, FilterRequest status);
        Task<ObjectModelResponse> GetDetailsRequest(PaginationRequest model, Guid id);
        Task<ObjectModelResponse> CreateRequest(RequestRequest model);
        Task<ObjectModelResponse> UpdateRequest(Guid id, RequestUpdateRequest model);
        Task<ObjectModelResponse> DisableRequest(Guid id);
        Task<ResponseModel<TechnicianRequestResponse>> GetTechnicianRequest(PaginationRequest model, Guid id);
        Task<ObjectModelResponse> MappingTechnicianRequest(Guid request_id, Guid technician_id);
        Task<ResponseModel<DeviceResponse>> GetDeviceRequest(PaginationRequest model, Guid id);
    }
    public class RequestService : IRequestService
    {

        private readonly Database_UPODContext _context;
        public RequestService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<RequestListResponse>> GetListRequests(PaginationRequest model, FilterRequest status)
        {
            var requests = new List<RequestListResponse>();
            if (status.filter_status == null)
            {
                requests = await _context.Requests.Where(a => a.IsDelete == false).Select(a => new RequestListResponse
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
                        percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                        percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                        percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
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
                    priority = a.Priority,
                    estimation = a.Estimation,
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                requests = await _context.Requests.Where(a => a.IsDelete == false && a.RequestStatus!.Equals(status.filter_status)).Select(a => new RequestListResponse
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
                        percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                        percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                        percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
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
                    priority = a.Priority,
                    estimation = a.Estimation,
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

        public async Task<ResponseModel<DeviceResponse>> GetDeviceRequest(PaginationRequest model, Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var agency = await _context.Agencies.Where(a => a.Id.Equals(request!.AgencyId) && a.IsDelete == false).FirstOrDefaultAsync();
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
                update_date = a.UpdateDate

            }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<DeviceResponse>(device)
            {
                Total = device.Count,
                Type = "Devices"
            };
        }
        public async Task<ResponseModel<TechnicianRequestResponse>> GetTechnicianRequest(PaginationRequest model, Guid id)
        {

            var request = await _context.Requests.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            var agency = await _context.Agencies.Where(a => a.Id.Equals(request!.AgencyId)).FirstOrDefaultAsync();
            var area = await _context.Areas.Where(a => a.Id.Equals(agency!.AreaId)).FirstOrDefaultAsync();
            var service = await _context.Services.Where(a => a.Id.Equals(request!.ServiceId)).FirstOrDefaultAsync();
            var technicans = await _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
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
                rating_avg = a.Technician.RatingAvg,
                is_busy = a.Technician.IsBusy,
                is_delete = a.Technician.IsDelete,
                create_date = a.Technician.CreateDate,
                update_date = a.Technician.UpdateDate,

            }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<TechnicianRequestResponse>(technicans)
            {
                Total = technicans.Count,
                Type = "Technicians"
            };
        }
        public async Task<ObjectModelResponse> GetDetailsRequest(PaginationRequest model, Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            var agency = await _context.Agencies.Where(a => a.Id.Equals(request!.AgencyId)).FirstOrDefaultAsync();
            var area = await _context.Areas.Where(a => a.Id.Equals(agency!.AreaId)).FirstOrDefaultAsync();
            var service = await _context.Services.Where(a => a.Id.Equals(request!.ServiceId)).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(a => a.Id.Equals(agency!.TechnicianDefault)).FirstOrDefaultAsync();
            var request_details = new RequestResponse();
            if (technician!.IsBusy == true)
            {
                request_details = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new RequestResponse
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
                        percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                        percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                        percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
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
                    technicican_default = _context.Skills.Where(a => a.ServiceId.Equals(service!.Id)
                    && a.Technician!.AreaId.Equals(area!.Id)
                    && a.Technician.IsBusy == false
                    && a.Technician.IsDelete == false).Select(a => new TechnicianViewResponse
                    {
                        id = a.TechnicianId,
                        code = a.Technician!.Code,
                        name = a.Technician.TechnicianName

                    }).OrderByDescending(x => x.code).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList(),
                    priority = a.Priority,
                    estimation = a.Estimation,
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                }).FirstOrDefaultAsync();
            }
            else
            {
                request_details = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new RequestResponse
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
                        percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                        percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                        percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
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
                    technicican_default = _context.Technicians.Where(a => a.Id.Equals(agency!.TechnicianDefault) && a.IsDelete == false)
                    .Select(a => new TechnicianViewResponse
                    {
                        id = a.Id,
                        code = a.Code,
                        name = a.TechnicianName

                    }).ToList(),
                    priority = a.Priority,
                    estimation = a.Estimation,
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                }).FirstOrDefaultAsync();

            }

            return new ObjectModelResponse(request_details!)
            {
                Type = "Request"
            };

        }
        public async Task<ObjectModelResponse> MappingTechnicianRequest(Guid request_id, Guid technician_id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(request_id)).FirstOrDefaultAsync();
            request!.CurrentTechnicianId = technician_id;
            request.StartTime = DateTime.Now;
            request.RequestStatus = ProcessStatus.PREPARING.ToString();
            _context.Requests.Update(request);
            var data = new MappingTechnicianResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new MappingTechnicianResponse
                {
                    id = request_id,
                    technician_id = technician_id,
                    request_status = request.RequestStatus,
                    start_time = request.StartTime
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
            var num = await GetLastCode();
            var code = CodeHelper.GeneratorCode("RE", num + 1);
            var request = new Request
            {
                Id = Guid.NewGuid(),
                Code = code,
                RequestName = model.request_name,
                CustomerId = _context.Agencies.Where(a => a.Id.Equals(model.agency_id)).Select(a => a.CustomerId).FirstOrDefault(),
                ServiceId = model.service_id,
                AgencyId = model.agency_id,
                RequestDesciption = model.request_description,
                RequestStatus = ProcessStatus.PENDING.ToString(),
                Estimation = model.estimation,
                Phone = _context.Agencies.Where(x => x.Id.Equals(model.agency_id)).Select(x => x.Telephone).FirstOrDefault(),
                Priority = model.priority,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Token = null,
                Img = null,
                ExceptionSource = null,
                IsDelete = false,
                Feedback = "",
                Rating = 0,
                CurrentTechnicianId = null,
                StartTime = null,
                EndTime = null,
            };
            var data = new RequestCreateResponse();
            var message = "blank";
            var status = 500;
            var id = await _context.Requests.Where(x => x.Id.Equals(request.Id)).FirstOrDefaultAsync();
            if (id != null)
            {
                status = 400;
                message = "Id is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
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
                        estimation = request.Estimation,
                        phone = request.Phone,
                        priority = request.Priority,
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
                Estimation = model.estimation,
                Phone = model.phone,
                Priority = model.priority,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
                Token = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.Token).FirstOrDefault(),
                Img = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.Img).FirstOrDefault(),
                ExceptionSource = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.ExceptionSource).FirstOrDefault(),
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
                        estimation = request.Estimation,
                        phone = request.Phone,
                        priority = request.Priority,
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

    }
}
