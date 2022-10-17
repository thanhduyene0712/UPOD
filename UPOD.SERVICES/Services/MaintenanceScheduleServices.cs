using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Services;

namespace UPOD.SERVICES.Services
{
    public interface IMaintenanceScheduleService
    {
        Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedules(PaginationRequest model, FilterRequest value);
        Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedulesTechnician(PaginationRequest model, Guid id);
        Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedulesAgency(PaginationRequest model, Guid id);
        Task<ObjectModelResponse> UpdateMaintenanceSchedule(Guid id, MaintenanceScheduleRequest model);
        Task<ObjectModelResponse> DisableMaintenanceSchedule(Guid id);
    }

    public class MaintenanceScheduleServices : IMaintenanceScheduleService
    {
        private readonly Database_UPODContext _context;
        public MaintenanceScheduleServices(Database_UPODContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedules(PaginationRequest model, FilterRequest value)
        {
            var maintenanceSchedules = new List<MaintenanceScheduleResponse>();
            if (value.search != null)
            {
                maintenanceSchedules = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false
                 && (a.Name!.Contains(value.search)
                 || a.Status!.Equals(value.search)
                 || a.Code!.Contains(value.search))).Select(a => new MaintenanceScheduleResponse
                 {
                     id = a.Id,
                     code = a.Code,
                     name = a.Name,
                     description = a.Description,
                     is_delete = a.IsDelete,
                     create_date = a.CreateDate,
                     update_date = a.UpdateDate,
                     maintain_time = a.MaintainTime,
                     status = a.Status,
                     technician = new TechnicianViewResponse
                     {
                         id = a.TechnicianId,
                         name = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                         code = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.Code).FirstOrDefault(),

                     },
                     agency = new AgencyViewResponse
                     {
                         id = a.AgencyId,
                         code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                         agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                         address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                         phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Telephone).FirstOrDefault()
                     }
                 }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                maintenanceSchedules = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false).Select(a => new MaintenanceScheduleResponse
                {
                    id = a.Id,
                    code = a.Code,
                    name = a.Name,
                    description = a.Description,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    maintain_time = a.MaintainTime,
                    status = a.Status,
                    technician = new TechnicianViewResponse
                    {
                        id = a.TechnicianId,
                        name = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.Code).FirstOrDefault(),

                    },
                    agency = new AgencyViewResponse
                    {
                        id = a.AgencyId,
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Telephone).FirstOrDefault()
                    }
                }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<MaintenanceScheduleResponse>(maintenanceSchedules)
            {
                Total = maintenanceSchedules.Count,
                Type = "MaintenanceSchedules"
            };

        }
        public async Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedulesTechnician(PaginationRequest model, Guid id)
        {
            var maintenanceSchedules = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false && a.TechnicianId.Equals(id)).Select(a => new MaintenanceScheduleResponse
            {
                id = a.Id,
                code = a.Code,
                name = a.Name,
                description = a.Description,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
                maintain_time = a.MaintainTime,
                status = a.Status,
                technician = new TechnicianViewResponse
                {
                    id = a.TechnicianId,
                    name = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    code = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.Code).FirstOrDefault(),

                },
                agency = new AgencyViewResponse
                {
                    id = a.AgencyId,
                    code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                    agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                    address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                    phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Telephone).FirstOrDefault()
                }
            }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<MaintenanceScheduleResponse>(maintenanceSchedules)
            {
                Total = maintenanceSchedules.Count,
                Type = "MaintenanceSchedules"
            };
        }
        public async Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedulesAgency(PaginationRequest model, Guid id)
        {
            var maintenanceSchedules = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).Select(a => new MaintenanceScheduleResponse
            {
                id = a.Id,
                code = a.Code,
                name = a.Name,
                description = a.Description,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
                maintain_time = a.MaintainTime,
                status = a.Status,
                technician = new TechnicianViewResponse
                {
                    id = a.TechnicianId,
                    name = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    code = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.Code).FirstOrDefault(),

                },
                agency = new AgencyViewResponse
                {
                    id = a.AgencyId,
                    code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                    agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                    address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                    phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Telephone).FirstOrDefault()
                }
            }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<MaintenanceScheduleResponse>(maintenanceSchedules)
            {
                Total = maintenanceSchedules.Count,
                Type = "MaintenanceSchedules"
            };
        }
        public async Task<ObjectModelResponse> UpdateMaintenanceSchedule(Guid id, MaintenanceScheduleRequest model)
        {
            var maintenanceSchedule = await _context.MaintenanceSchedules.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            maintenanceSchedule!.Description = model.description;
            maintenanceSchedule!.MaintainTime = model.maintain_time;
            maintenanceSchedule.UpdateDate = DateTime.Now;
            _context.MaintenanceSchedules.Update(maintenanceSchedule);
            var data = new MaintenanceScheduleResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new MaintenanceScheduleResponse
                {
                    id = maintenanceSchedule.Id,
                    code = maintenanceSchedule.Code,
                    name = maintenanceSchedule.Name,
                    description = maintenanceSchedule.Description,
                    is_delete = maintenanceSchedule.IsDelete,
                    create_date = maintenanceSchedule.CreateDate,
                    update_date = maintenanceSchedule.UpdateDate,
                    maintain_time = maintenanceSchedule.MaintainTime,
                    status = maintenanceSchedule.Status,
                    technician = new TechnicianViewResponse
                    {
                        id = maintenanceSchedule.TechnicianId,
                        name = _context.Technicians.Where(x => x.Id.Equals(maintenanceSchedule.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(maintenanceSchedule.TechnicianId)).Select(a => a.Code).FirstOrDefault(),

                    },
                    agency = new AgencyViewResponse
                    {
                        id = maintenanceSchedule.AgencyId,
                        code = _context.Agencies.Where(x => x.Id.Equals(maintenanceSchedule.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(maintenanceSchedule.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(maintenanceSchedule.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(maintenanceSchedule.AgencyId)).Select(a => a.Telephone).FirstOrDefault()
                    }
                };
            }
            return new ObjectModelResponse(data)
            {
                Type = "MaintenanceSchedule"
            };
        }
        public async Task<ObjectModelResponse> DisableMaintenanceSchedule(Guid id)
        {
            var maintenanceSchedule = await _context.MaintenanceSchedules.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            maintenanceSchedule!.IsDelete = true;
            _context.MaintenanceSchedules.Update(maintenanceSchedule);
            var data = new MaintenanceScheduleResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new MaintenanceScheduleResponse
                {
                    id = maintenanceSchedule.Id,
                    code = maintenanceSchedule.Code,
                    name = maintenanceSchedule.Name,
                    description = maintenanceSchedule.Description,
                    is_delete = maintenanceSchedule.IsDelete,
                    create_date = maintenanceSchedule.CreateDate,
                    update_date = maintenanceSchedule.UpdateDate,
                    maintain_time = maintenanceSchedule.MaintainTime,
                    status = maintenanceSchedule.Status,
                    technician = new TechnicianViewResponse
                    {
                        id = maintenanceSchedule.TechnicianId,
                        name = _context.Technicians.Where(x => x.Id.Equals(maintenanceSchedule.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(maintenanceSchedule.TechnicianId)).Select(a => a.Code).FirstOrDefault(),

                    },
                    agency = new AgencyViewResponse
                    {
                        id = maintenanceSchedule.AgencyId,
                        code = _context.Agencies.Where(x => x.Id.Equals(maintenanceSchedule.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(maintenanceSchedule.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(maintenanceSchedule.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(maintenanceSchedule.AgencyId)).Select(a => a.Telephone).FirstOrDefault()
                    }
                };
            }
            return new ObjectModelResponse(data)
            {
                Type = "MaintenanceSchedule"
            };
        }
    }
}
