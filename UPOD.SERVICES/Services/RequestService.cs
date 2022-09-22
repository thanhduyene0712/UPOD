using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Enum;

namespace UPOD.SERVICES.Services
{
    public interface IRequestService
    {
        Task<ResponseModel<RequestResponse>> GetListRequest(PaginationRequest model);
        Task<ResponseModel<RequestDetailResponse>> GetDetailRequest(Guid id);
        Task<ResponseModel<RequestCreateResponse>> CreateRequest(RequestRequest model);
        Task<ResponseModel<RequestCreateResponse>> UpdateRequest(Guid id, RequestUpdateRequest model);
        Task<ResponseModel<RequestDisableResponse>> DisableRequest(Guid id);
        Task<ResponseModel<TechnicianRequestResponse>> GetTechnicanRequest(PaginationRequest model, Guid id);
        Task<ResponseModel<MappingTechnicianResponse>> MappingTechnicianRequest(Guid request_id, Guid technician_id);
        Task<ResponseModel<DeviceResponse>> GetDeviceRequest(PaginationRequest model, Guid id);
    }
    public class RequestService : IRequestService
    {

        private readonly Database_UPODContext _context;
        public RequestService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<RequestResponse>> GetListRequest(PaginationRequest model)
        {
            var request = await _context.Requests.Where(a => a.IsDelete == false).Select(a => new RequestResponse
            {
                id = a.Id,
                request_name = a.RequestName,
                company_name = _context.Companies.Where(x => x.Id.Equals(a.CompanyId)).Select(x => x.CompanyName).FirstOrDefault(),
                agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                estimation = a.Estimation,
                request_status = a.RequestStatus,
                service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),

            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<RequestResponse>(request)
            {
                Total = request.Count,
                Type = "Requests"
            };
        }
        public async Task<ResponseModel<DeviceResponse>> GetDeviceRequest(PaginationRequest model, Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var agency = await _context.Agencies.Where(a => a.Id.Equals(request.AgencyId) && a.IsDelete == false).FirstOrDefaultAsync();
            var device = await _context.AgencyDevices.Where(a => a.AgencyId.Equals(agency.Id)).Select(a => new DeviceResponse
            {
                id = a.Device.Id,
                conpany_id = a.Device.ConpanyId,
                devicetype_id = a.Device.DeviceTypeId,
                device_name = a.Device.DeviceName,
                device_code = a.Device.DeviceCode,
                guaranty_start_date = a.Device.GuarantyStartDate,
                guaranty_end_date = a.Device.GuarantyEndDate,
                ip = a.Device.Ip,
                port = a.Device.Port,
                device_account = a.Device.DeviceAccount,
                device_password = a.Device.DevicePassword,
                setting_date = a.Device.SettingDate,
                other = a.Device.Other,
                is_delete = a.Device.IsDelete,
                create_date = a.Device.CreateDate,
                update_date = a.Device.UpdateDate

            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<DeviceResponse>(device)
            {
                Total = device.Count,
                Type = "Devices"
            };
        }
        public async Task<ResponseModel<TechnicianRequestResponse>> GetTechnicanRequest(PaginationRequest model, Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            var agency = await _context.Agencies.Where(a => a.Id.Equals(request.AgencyId)).FirstOrDefaultAsync();
            var area = await _context.Areas.Where(a => a.Id.Equals(agency.AreaId)).FirstOrDefaultAsync();
            var service = await _context.Services.Where(a => a.Id.Equals(request.ServiceId)).FirstOrDefaultAsync();
            var technicans = await _context.Skills.Where(a => a.ServiceId.Equals(service.Id)
            && a.Technican.AreaId.Equals(area.Id)
            && a.Technican.IsBusy == false
            && a.Technican.IsDelete == false).Select(a => new TechnicianRequestResponse
            {
                id = a.TechnicanId,
                area_id = a.Technican.AreaId,
                technican_name = a.Technican.TechnicanName,
                account_id = a.Technican.AccountId,
                telephone = a.Technican.Telephone,
                email = a.Technican.Email,
                gender = a.Technican.Gender,
                address = a.Technican.Address,
                rating_avg = a.Technican.RatingAvg,
                is_busy = a.Technican.IsBusy,
                is_delete = a.Technican.IsDelete,
                create_date = a.Technican.CreateDate,
                update_date = a.Technican.UpdateDate,

            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<TechnicianRequestResponse>(technicans)
            {
                Total = technicans.Count,
                Type = "Technicians"
            };
        }
        public async Task<ResponseModel<RequestDetailResponse>> GetDetailRequest(Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new RequestDetailResponse
            {
                id = id,
                company_name = _context.Companies.Where(x => x.Id.Equals(a.CompanyId)).Select(x => x.CompanyName).FirstOrDefault(),
                agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                address_service = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                description_serivce = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(x => x.Desciption).FirstOrDefault(),
                request_name = a.RequestName,
                estimation = a.Estimation,
                description_request = a.RequestDesciption,
                priority = a.Priority,
                phone = a.Phone,
                request_status = a.RequestStatus,
            }).ToListAsync();
            return new ResponseModel<RequestDetailResponse>(request)
            {
                Total = request.Count,
                Type = "Request"
            };
        }
        public async Task<ResponseModel<MappingTechnicianResponse>> MappingTechnicianRequest(Guid request_id, Guid technician_id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(request_id)).FirstOrDefaultAsync();
            request.CurrentTechnicanId = technician_id;
            request.StartTime = DateTime.Now;
            request.RequestStatus = (int)ProcessStatus.Preparing;
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
            var list = new List<MappingTechnicianResponse>();
            list.Add(new MappingTechnicianResponse
            {
                id = request_id,
                technician_id = technician_id,
                request_status = request.RequestStatus,
                start_time = request.StartTime
            });
            return new ResponseModel<MappingTechnicianResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Request"
            };
        }
        public async Task<ResponseModel<RequestCreateResponse>> CreateRequest(RequestRequest model)
        {
            var request = new Request
            {
                Id = Guid.NewGuid(),
                RequestName = model.request_name,
                CompanyId = _context.Agencies.Where(a => a.Id.Equals(model.agency_id)).Select(a => a.CompanyId).FirstOrDefault(),
                ServiceId = model.service_id,
                AgencyId = model.agency_id,
                RequestDesciption = model.request_description,
                RequestStatus = (int)ProcessStatus.Pending,
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
                CurrentTechnicanId = null,
                StartTime = null,
                EndTime = null,
            };
            var list = new List<RequestCreateResponse>();
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
                await _context.SaveChangesAsync();
                list.Add(new RequestCreateResponse
                {
                    request_name = request.RequestName,
                    request_description = request.RequestDesciption,
                    estimation = request.Estimation,
                    phone = request.Phone,
                    priority = request.Priority,
                    agency_name = _context.Agencies.Where(x => x.Id.Equals(request.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                    service_name = _context.Services.Where(x => x.Id.Equals(request.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                });
            }
            return new ResponseModel<RequestCreateResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Request"
            };
        }
        public async Task<ResponseModel<RequestCreateResponse>> UpdateRequest(Guid id, RequestUpdateRequest model)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id)).Select(x => new Request
            {
                Id = id,
                RequestName = model.request_name,
                CompanyId = _context.Agencies.Where(a => a.Id.Equals(model.agency_id)).Select(a => a.CompanyId).FirstOrDefault(),
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
                CurrentTechnicanId = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.CurrentTechnicanId).FirstOrDefault(),
                StartTime = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.StartTime).FirstOrDefault(),
                EndTime = _context.Requests.Where(a => a.Id.Equals(id)).Select(x => x.EndTime).FirstOrDefault(),
            }).FirstOrDefaultAsync();
            var message = "blank";
            var status = 500;
            var list = new List<RequestCreateResponse>();
            if (request.RequestStatus != 1)
            {
                status = 400;
                message = "You just update when request status is pending";
            }
            else
            {
                status = 201;
                message = "Successfully";
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
                list.Add(new RequestCreateResponse
                {
                    request_name = request.RequestName,
                    request_description = request.RequestDesciption,
                    estimation = request.Estimation,
                    phone = request.Phone,
                    priority = request.Priority,
                    agency_name = _context.Agencies.Where(x => x.Id.Equals(request.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                    service_name = _context.Services.Where(x => x.Id.Equals(request.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                });
            }
            return new ResponseModel<RequestCreateResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Request"
            };
        }
        public async Task<ResponseModel<RequestDisableResponse>> DisableRequest(Guid id)
        {
            var request = await _context.Requests.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            request.IsDelete = true;

            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
            var list = new List<RequestDisableResponse>();
            list.Add(new RequestDisableResponse
            {
                id = request.Id,
                isDelete = request.IsDelete,
            });
            return new ResponseModel<RequestDisableResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Request"
            };
        }

    }
}
