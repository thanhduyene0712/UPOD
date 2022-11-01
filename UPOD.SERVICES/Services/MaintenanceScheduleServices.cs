using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Enum;
using UPOD.SERVICES.Services;

namespace UPOD.SERVICES.Services
{
    public interface IMaintenanceScheduleService
    {
        Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedules(PaginationRequest model, FilterStatusRequest value);
        Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedulesTechnician(PaginationRequest model, Guid id, FilterStatusRequest value);
        Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedulesAgency(PaginationRequest model, Guid id, FilterStatusRequest value);
        Task<ObjectModelResponse> UpdateMaintenanceSchedule(Guid id, MaintenanceScheduleRequest model);
        Task<ObjectModelResponse> MaintainingSchedule(Guid id);
        Task<ObjectModelResponse> DisableMaintenanceSchedule(Guid id);
        Task<Dictionary<Guid, Guid>> GetMaintenanceSchedulesNotify();
        Task SetStatus(ScheduleStatus status, Guid scheduleId);
        Task<Dictionary<Guid, Guid>> GetMaintenanceSchedulesNotifyMissing();
    }

    public class MaintenanceScheduleServices : IMaintenanceScheduleService
    {
        private readonly Database_UPODContext _context;
        public MaintenanceScheduleServices(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ObjectModelResponse> MaintainingSchedule(Guid id)
        {
            var maintenanceSchedule = await _context.MaintenanceSchedules.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(a => a.Id.Equals(maintenanceSchedule!.TechnicianId) && a.IsDelete == false).FirstOrDefaultAsync();
            technician!.IsBusy = true;
            maintenanceSchedule!.Status = ScheduleStatus.MAINTAINING.ToString();
            maintenanceSchedule.StartDate = DateTime.UtcNow.AddHours(7);
            _context.MaintenanceSchedules.Update(maintenanceSchedule);
            _context.Technicians.Update(technician);
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
                    service = new ServiceViewResponse
                    {
                        id = maintenanceSchedule.ServiceId,
                        service_name = _context.Services.Where(x => x.Id.Equals(maintenanceSchedule.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(maintenanceSchedule.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(maintenanceSchedule.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                        frequency_maintain = _context.ContractServices.Where(x => x.ServiceId.Equals(maintenanceSchedule.ServiceId) && x.ContractId.Equals(maintenanceSchedule.ContractId)).Select(a => a.FrequencyMaintain).FirstOrDefault(),

                    },
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
                Status = 201,
                Type = "MaintenanceSchedule"
            };
        }
        public async Task<Dictionary<Guid, Guid>> GetMaintenanceSchedulesNotify()
        {

            var todaySchedules = await _context.MaintenanceSchedules.Where(a => (a.MaintainTime!.Value.Date == DateTime.UtcNow.AddHours(7).Date) && a.IsDelete == false).ToListAsync();
            var rs = new Dictionary<Guid, Guid>();
            foreach (var item in todaySchedules)
            {
                rs.Add(item.TechnicianId!.Value, item.Id);
            }
            return rs;
        }
        public async Task<Dictionary<Guid, Guid>> GetMaintenanceSchedulesNotifyMissing()
        {
            var maintainSchedule = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false).ToListAsync();
            var rs = new Dictionary<Guid, Guid>();
            foreach (var item in maintainSchedule)
            {
                var missingDate = DateTime.UtcNow.AddHours(7) - item.MaintainTime;
                var date = missingDate!.Value.Days;
                if (date >= 5)
                {
                    var missingSchedules = await _context.MaintenanceSchedules.Where(a => a.Status!.Equals("NOTIFIED") && a.IsDelete == false).ToListAsync();

                    foreach (var item1 in missingSchedules)
                    {
                        rs.Add(item.TechnicianId!.Value, item.Id);
                    }
                }
            }


            return rs;
        }
        public async Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedules(PaginationRequest model, FilterStatusRequest value)
        {
            var total = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false).ToListAsync();
            var maintenanceSchedules = new List<MaintenanceScheduleResponse>();
            if (value.search == null && value.status == null)
            {
                total = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false).ToListAsync();
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
                    service = new ServiceViewResponse
                    {
                        id = a.ServiceId,
                        service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                        frequency_maintain = _context.ContractServices.Where(x => x.ServiceId.Equals(a.ServiceId) && x.ContractId.Equals(a.ContractId)).Select(a => a.FrequencyMaintain).FirstOrDefault(),

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

                if (value.search == null)
                {
                    value.search = "";
                }
                if (value.status == null)
                {
                    value.status = "";
                }
                var agency_name = await _context.Agencies.Where(a => a.AgencyName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var customer_name = await _context.Customers.Where(a => a.Name!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var contract_name = await _context.Contracts.Where(a => a.ContractName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var service_name = await _context.Services.Where(a => a.ServiceName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var technician_name = await _context.Technicians.Where(a => a.TechnicianName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                total = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false
                 && (a.Status!.Contains(value.status)
                 && (a.Name!.Contains(value.search)
                 || a.Code!.Contains(value.search)
                 || a.AgencyId!.Equals(agency_name)
                 || a.TechnicianId!.Equals(technician_name)
                 || a.ContractId!.Equals(contract_name)
                 || a.ServiceId!.Equals(service_name)))).ToListAsync();
                maintenanceSchedules = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false
                 && (a.Status!.Contains(value.status)
                 && (a.Name!.Contains(value.search)
                 || a.Code!.Contains(value.search)
                 || a.AgencyId!.Equals(agency_name)
                 || a.TechnicianId!.Equals(technician_name)
                 || a.ContractId!.Equals(contract_name)
                 || a.ServiceId!.Equals(service_name)))).Select(a => new MaintenanceScheduleResponse
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
                     service = new ServiceViewResponse
                     {
                         id = a.ServiceId,
                         service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                         code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                         description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                         frequency_maintain = _context.ContractServices.Where(x => x.ServiceId.Equals(a.ServiceId) && x.ContractId.Equals(a.ContractId)).Select(a => a.FrequencyMaintain).FirstOrDefault(),

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
                Total = total.Count,
                Type = "MaintenanceSchedules"
            };

        }
        public async Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedulesTechnician(PaginationRequest model, Guid id, FilterStatusRequest value)
        {
            var total = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false && a.TechnicianId.Equals(id)).ToListAsync();
            var maintenanceSchedules = new List<MaintenanceScheduleResponse>();
            if (value.search == null && value.status == null)
            {
                total = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false && a.TechnicianId.Equals(id)).ToListAsync();
                maintenanceSchedules = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false && a.TechnicianId.Equals(id)).Select(a => new MaintenanceScheduleResponse
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
                    service = new ServiceViewResponse
                    {
                        id = a.ServiceId,
                        service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                        frequency_maintain = _context.ContractServices.Where(x => x.ServiceId.Equals(a.ServiceId) && x.ContractId.Equals(a.ContractId)).Select(a => a.FrequencyMaintain).FirstOrDefault(),

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
                if (value.search == null)
                {
                    value.search = "";
                }
                if (value.status == null)
                {
                    value.status = "";
                }
                var agency_name = await _context.Agencies.Where(a => a.AgencyName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var customer_name = await _context.Customers.Where(a => a.Name!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var contract_name = await _context.Contracts.Where(a => a.ContractName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var service_name = await _context.Services.Where(a => a.ServiceName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var technician_name = await _context.Technicians.Where(a => a.TechnicianName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                total = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false
                 && a.TechnicianId.Equals(id)
                 && (a.Status!.Contains(value.status)
                 && (a.Name!.Contains(value.search)
                 || a.Code!.Contains(value.search)
                 || a.AgencyId!.Equals(agency_name)
                 || a.TechnicianId!.Equals(technician_name)
                 || a.ContractId!.Equals(contract_name)
                 || a.ServiceId!.Equals(service_name)))).ToListAsync();
                maintenanceSchedules = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false
                 && a.TechnicianId.Equals(id)
                 && (a.Status!.Contains(value.status)
                 && (a.Name!.Contains(value.search)
                 || a.Code!.Contains(value.search)
                 || a.AgencyId!.Equals(agency_name)
                 || a.TechnicianId!.Equals(technician_name)
                 || a.ContractId!.Equals(contract_name)
                 || a.ServiceId!.Equals(service_name)))).Select(a => new MaintenanceScheduleResponse
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
                     service = new ServiceViewResponse
                     {
                         id = a.ServiceId,
                         service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                         code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                         description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                         frequency_maintain = _context.ContractServices.Where(x => x.ServiceId.Equals(a.ServiceId) && x.ContractId.Equals(a.ContractId)).Select(a => a.FrequencyMaintain).FirstOrDefault(),

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
                Total = total.Count,
                Type = "MaintenanceSchedules"
            };
        }
        public async Task<ResponseModel<MaintenanceScheduleResponse>> GetListMaintenanceSchedulesAgency(PaginationRequest model, Guid id, FilterStatusRequest value)
        {
            var total = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).ToListAsync();
            var maintenanceSchedules = new List<MaintenanceScheduleResponse>();
            if (value.search == null && value.status == null)
            {
                total = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).ToListAsync();
                maintenanceSchedules = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).Select(a => new MaintenanceScheduleResponse
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
                    service = new ServiceViewResponse
                    {
                        id = a.ServiceId,
                        service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                        frequency_maintain = _context.ContractServices.Where(x => x.ServiceId.Equals(a.ServiceId) && x.ContractId.Equals(a.ContractId)).Select(a => a.FrequencyMaintain).FirstOrDefault(),

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
                if (value.search == null)
                {
                    value.search = "";
                }
                if (value.status == null)
                {
                    value.status = "";
                }
                var agency_name = await _context.Agencies.Where(a => a.AgencyName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var customer_name = await _context.Customers.Where(a => a.Name!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var contract_name = await _context.Contracts.Where(a => a.ContractName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var service_name = await _context.Services.Where(a => a.ServiceName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                var technician_name = await _context.Technicians.Where(a => a.TechnicianName!.Contains(value.search!.Trim())).Select(a => a.Id).FirstOrDefaultAsync();
                total = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false
                 && a.AgencyId.Equals(id)
                 && (a.Status!.Contains(value.status)
                 && (a.Name!.Contains(value.search)
                 || a.Code!.Contains(value.search)
                 || a.AgencyId!.Equals(agency_name)
                 || a.TechnicianId!.Equals(technician_name)
                 || a.ContractId!.Equals(contract_name)
                 || a.ServiceId!.Equals(service_name)))).ToListAsync();
                maintenanceSchedules = await _context.MaintenanceSchedules.Where(a => a.IsDelete == false
                 && a.AgencyId.Equals(id)
                 && (a.Status!.Contains(value.status)
                 && (a.Name!.Contains(value.search)
                 || a.Code!.Contains(value.search)
                 || a.AgencyId!.Equals(agency_name)
                 || a.TechnicianId!.Equals(technician_name)
                 || a.ContractId!.Equals(contract_name)
                 || a.ServiceId!.Equals(service_name)))).Select(a => new MaintenanceScheduleResponse
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
                     service = new ServiceViewResponse
                     {
                         id = a.ServiceId,
                         service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                         code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                         description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                         frequency_maintain = _context.ContractServices.Where(x => x.ServiceId.Equals(a.ServiceId) && x.ContractId.Equals(a.ContractId)).Select(a => a.FrequencyMaintain).FirstOrDefault(),

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
                Total = total.Count,
                Type = "MaintenanceSchedules"
            };
        }
        public async Task<ObjectModelResponse> UpdateMaintenanceSchedule(Guid id, MaintenanceScheduleRequest model)
        {
            var maintenanceSchedule = await _context.MaintenanceSchedules.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            maintenanceSchedule!.Description = model.description;
            maintenanceSchedule!.MaintainTime = model.maintain_time;
            maintenanceSchedule!.Status = ScheduleStatus.SCHEDULED.ToString();
            maintenanceSchedule.UpdateDate = DateTime.UtcNow.AddHours(7);
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
                    service = new ServiceViewResponse
                    {
                        id = maintenanceSchedule.ServiceId,
                        service_name = _context.Services.Where(x => x.Id.Equals(maintenanceSchedule.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(maintenanceSchedule.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(maintenanceSchedule.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                        frequency_maintain = _context.ContractServices.Where(x => x.ServiceId.Equals(maintenanceSchedule.ServiceId) && x.ContractId.Equals(maintenanceSchedule.ContractId)).Select(a => a.FrequencyMaintain).FirstOrDefault(),

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
                Status = 201,
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
                    service = new ServiceViewResponse
                    {
                        id = maintenanceSchedule.ServiceId,
                        service_name = _context.Services.Where(x => x.Id.Equals(maintenanceSchedule.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(maintenanceSchedule.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(maintenanceSchedule.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                        frequency_maintain = _context.ContractServices.Where(x => x.ServiceId.Equals(maintenanceSchedule.ServiceId) && x.ContractId.Equals(maintenanceSchedule.ContractId)).Select(a => a.FrequencyMaintain).FirstOrDefault(),

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
                Status = 201,
                Type = "MaintenanceSchedule"
            };
        }

        public async Task SetStatus(ScheduleStatus status, Guid scheduleId)
        {
            var maintainStatus = await _context.MaintenanceSchedules.Where(a => a.Id.Equals(scheduleId) && a.IsDelete == false).FirstOrDefaultAsync();
            maintainStatus!.Status = status.ToString();
            await _context.SaveChangesAsync();
        }
    }
}
