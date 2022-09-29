using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.REPOSITORIES.Services;
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
        Task<ResponseModel<TicketResponse>> CreateTicket(Guid id, TicketRequests model);
        Task<ResponseModel<RequestResponse>> GetListRequestsOfTechnician(PaginationRequest model, Guid id);
    }

    public class TechnicianService : ITechnicianService
    {
        private readonly Database_UPODContext _context;
        public TechnicianService(Database_UPODContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<TechnicianResponse>> GetListTechnicians(PaginationRequest model)
        {
            var technicians = await _context.Technicians.Where(a => a.IsDelete == false).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).Select(a => new TechnicianResponse
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
            }).OrderByDescending(x => x.update_date).ToListAsync();

            return new ResponseModel<TechnicianResponse>(technicians)
            {
                Total = technicians.Count,
                Type = "Technicians"
            };
        }
        public async Task<ResponseModel<RequestResponse>> GetListRequestsOfTechnician(PaginationRequest model, Guid id)
        {
            var request = await _context.Requests.Where(a => a.IsDelete == false && a.CurrentTechnicianId.Equals(id))
                .Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).Select(a => new RequestResponse
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

                }).OrderByDescending(x => x.update_date).ToListAsync();

            return new ResponseModel<RequestResponse>(request)
            {
                Total = request.Count,
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
        public async Task<ResponseModel<TicketResponse>> CreateTicket(Guid id, TicketRequests model)
        {

            var request = await _context.Requests.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(x => x.Id.Equals(request!.CurrentTechnicianId)).FirstOrDefaultAsync();
            request!.RequestStatus = ProcessStatus.RESOLVED.ToString();
            request.EndTime = DateTime.Now;
            request.UpdateDate = DateTime.Now;
            _context.Requests.Update(request);
            var list = new List<TicketResponse>();
            foreach (var item in model.tickets)
            {
                var num = await GetLastCode1();
                var code = CodeHelper.GeneratorCode("TI", num + 1);
                var device_id = Guid.NewGuid();
                while (true)
                {
                    var ticket_id = await _context.Technicians.Where(x => x.Id.Equals(device_id)).FirstOrDefaultAsync();
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
                    Id = Guid.NewGuid(),
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
                list.Add(new TicketResponse
                {
                    id = ticket.Id,
                    code = ticket.Code,
                    request_id = ticket.RequestId,
                    device_id = ticket.DeviceId,
                    description = ticket.Description,
                    solution = ticket.Solution,
                    is_delete = ticket.IsDelete,
                    create_by = ticket.CreateBy,
                    create_date = ticket.CreateDate,
                    update_date = ticket.UpdateDate,

                });
                var rs = await _context.SaveChangesAsync();

            }
            return new ResponseModel<TicketResponse>(list)
            {
                Total = list.Count,
                Type = "Ticket"
            };
        }
        public async Task<ObjectModelResponse> CreateTechnician(TechnicianRequest model)
        {
            var num = await GetLastCode();
            var code = CodeHelper.GeneratorCode("TE", num + 1);
            var technician = new Technician
            {
                Id = Guid.NewGuid(),
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
                var skill = new Skill
                {
                    Id = Guid.NewGuid(),
                    TechnicianId = technician.Id,
                    ServiceId = item,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                _context.Skills.Add(skill);
            }
            var data = new TechnicianUpdateResponse();
            var message = "blank";
            var status = 500;
            var technician_id = await _context.Technicians.Where(x => x.Id.Equals(technician.Id)).FirstOrDefaultAsync();
            if (technician_id != null)
            {
                status = 400;
                message = "TechnicianId is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;


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
            technician.UpdateDate = DateTime.Now;
            var data = new TechnicianUpdateResponse();
            _context.Technicians.Update(technician);
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
        public async Task<ObjectModelResponse> UpdateTechnician(Guid id, TechnicianRequest model)
        {
            var technician = await _context.Technicians.Where(a => a.Id.Equals(id)).Select(x => new Technician
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
