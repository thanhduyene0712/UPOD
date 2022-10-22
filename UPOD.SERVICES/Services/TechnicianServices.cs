using Microsoft.EntityFrameworkCore;
using System.Data;
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
        Task<ResponseModel<TechnicianResponse>> GetListTechnicians(PaginationRequest model);
        Task<ObjectModelResponse> GetDetailsTechnician(Guid id);
        Task<ObjectModelResponse> CreateTechnician(TechnicianRequest model);
        Task<ObjectModelResponse> UpdateTechnician(Guid id, TechnicianRequest model);
        Task<ObjectModelResponse> DisableTechnician(Guid id);
        Task<ResponseModel<DevicesOfRequestResponse>> CreateTicket(Guid id, ListTicketRequest model);
        Task<ResponseModel<RequestResponse>> GetListRequestsOfTechnician(PaginationRequest model, Guid id, FilterRequest value);
        Task<ResponseModel<DevicesOfRequestResponse>> GetDevicesByRequest(PaginationRequest model, Guid id);
        Task<ObjectModelResponse> ResolvingRequest(Guid id);
        Task<ObjectModelResponse> UpdateDeviceTicket(Guid id, TicketRequest model);
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
                create_date = a.UpdateDate,
            }).OrderByDescending(x => x.code).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).Distinct().ToListAsync();
            return new ResponseModel<DevicesOfRequestResponse>(device_of_request)
            {
                Total = total.Count,
                Type = "Devices"
            };
        }
        public async Task<ResponseModel<TechnicianResponse>> GetListTechnicians(PaginationRequest model)
        {
            var total = await _context.Technicians.Where(a => a.IsDelete == false).ToListAsync();
            var technicians = await _context.Technicians.Where(a => a.IsDelete == false).Select(a => new TechnicianResponse
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
                rating_avg = a.RatingAvg,
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

            return new ResponseModel<TechnicianResponse>(technicians)
            {
                Total = total.Count,
                Type = "Technicians"
            };
        }
        public async Task<ResponseModel<RequestResponse>> GetListRequestsOfTechnician(PaginationRequest model, Guid id, FilterRequest value)
        {
            var total = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)).ToListAsync();
            var request = new List<RequestResponse>();
            if (value.search == null)
            {
                total = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)).ToListAsync();
                request = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)).Select(a => new RequestResponse
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
                    priority = a.Priority,
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)
                && (a.RequestName!.Contains(value.search)
                || a.RequestStatus!.Contains(value.search)
                || a.Code!.Contains(value.search))).ToListAsync();
                request = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id)
                && (a.RequestName!.Contains(value.search)
                || a.RequestStatus!.Contains(value.search)
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
                    },
                    service = new ServiceViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                    },
                    priority = a.Priority,
                    request_status = a.RequestStatus,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            return new ResponseModel<RequestResponse>(request)
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
                rating_avg = a.RatingAvg,
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
        private async Task<int> GetLastCode1()
        {
            var ticket = await _context.Tickets.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(ticket!.Code!);
        }
        public async Task<ResponseModel<DevicesOfRequestResponse>> CreateTicket(Guid id, ListTicketRequest model)
        {

            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(x => x.Id.Equals(request!.CurrentTechnicianId)).FirstOrDefaultAsync();
            technician!.IsBusy = false;
            request!.RequestStatus = ProcessStatus.RESOLVED.ToString();
            request.EndTime = DateTime.Now;
            _context.Requests.Update(request);
            _context.Technicians.Update(technician);
            var list = new List<DevicesOfRequestResponse>();
            foreach (var item in model.ticket)
            {
                var num = await GetLastCode1();
                var code = CodeHelper.GeneratorCode("TI", num + 1);
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
                    Code = code,
                    RequestId = request.Id,
                    DeviceId = item.device_id,
                    Description = item.description,
                    Solution = item.solution,
                    IsDelete = false,
                    CreateBy = technician!.Id,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
                await _context.Tickets.AddAsync(ticket);
                list.Add(new DevicesOfRequestResponse
                {
                    ticket_id = ticket.Id,
                    device_id = ticket.DeviceId,
                    code = _context.Devices.Where(a => a.Id.Equals(ticket.DeviceId)).Select(a => a.Code).FirstOrDefault(),
                    name = _context.Devices.Where(a => a.Id.Equals(ticket.DeviceId)).Select(a => a.DeviceName).FirstOrDefault(),

                });
                await _context.SaveChangesAsync();

            }
            return new ResponseModel<DevicesOfRequestResponse>(list)
            {
                Total = list.Count,
                Type = "Devices"
            };
        }
        public async Task<ObjectModelResponse> UpdateDeviceTicket(Guid id, TicketRequest model)
        {

            var ticket = await _context.Tickets.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            ticket!.DeviceId = model.device_id;
            ticket!.Solution = model.solution;
            ticket!.Description = model.description;
            ticket!.UpdateDate = DateTime.Now;
            var rs = await _context.SaveChangesAsync();
            var data = new TicketViewResponse();
            if (rs > 0)
            {
                data = new TicketViewResponse
                {
                    id = ticket.Id,
                    device_id = ticket.DeviceId,
                    code = ticket.Code,
                    description = ticket.Description,
                    solution = ticket.Solution
                };
            }
            return new ObjectModelResponse(data!)
            {
                Type = "Device"
            };
        }
        public async Task<ObjectModelResponse> CreateTechnician(TechnicianRequest model)
        {
            var num = await GetLastCode();
            var code = CodeHelper.GeneratorCode("TE", num + 1);
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
                RatingAvg = model.rating_avg,
                IsBusy = false,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
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
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
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
                    rating_avg = technician.RatingAvg,
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
            technician.UpdateDate = DateTime.Now;
            var data = new TechnicianUpdateResponse();
            _context.Technicians.Update(technician);
            var technician_default = await _context.Agencies.Where(a => a.TechnicianId.Equals(id)).ToListAsync();
            foreach (var item in technician_default)
            {
                item.TechnicianId = null;
                _context.Agencies.Update(item);
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
                    rating_avg = technician.RatingAvg,
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
        public async Task<ObjectModelResponse> DisableDeviceOfTicket(Guid id)
        {
            var ticket = await _context.Tickets.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            ticket!.IsDelete = true;
            ticket.UpdateDate = DateTime.Now;
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
            request!.RequestStatus = ProcessStatus.RESOLVING.ToString();
            request.StartTime = DateTime.Now;
            _context.Requests.Update(request);
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
        public async Task<ObjectModelResponse> UpdateTechnician(Guid id, TechnicianRequest model)
        {
            var technician = await _context.Technicians.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(x => new Technician
            {
                Id = id,
                Code = x.Code,
                AreaId = model.area_id,
                TechnicianName = model.technician_name,
                AccountId = model.account_id,
                Telephone = model.telephone,
                Address = model.address,
                Email = model.email,
                Gender = model.gender,
                RatingAvg = model.rating_avg,
                IsBusy = x.IsBusy,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now
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
                    CreateDate = technician.CreateDate,
                    UpdateDate = DateTime.Now,
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
                    rating_avg = technician.RatingAvg,
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
