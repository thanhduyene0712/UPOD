using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Enum;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{
    public interface IMaintenanceReportService
    {
        Task<ResponseModel<MaintenanceReportResponse>> GetListMaintenanceReports(PaginationRequest model, FilterRequest value);
        Task<ObjectModelResponse> CreateMaintenanceReport(MaintenanceReportRequest model);
        Task<ResponseModel<MaintenanceReportServiceResponse>> CreateMaintenanceReportService(Guid id, ListMaintenanceReportServiceRequest model);
        //Task<ObjectModelResponse> UpdateMaintenanceReport(Guid id, MaintenanceReportRequest model);
        //Task<ObjectModelResponse> DisableMaintenanceReport(Guid id);
        Task<ObjectModelResponse> GetDetailsMaintenanceReport(Guid id);
    }

    public class MaintenanceReportServices : IMaintenanceReportService
    {
        private readonly Database_UPODContext _context;
        public MaintenanceReportServices(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<MaintenanceReportResponse>> GetListMaintenanceReports(PaginationRequest model, FilterRequest value)
        {
            var total = await _context.MaintenanceReports.Where(a => a.IsDelete == false).ToListAsync();
            var maintenanceReports = new List<MaintenanceReportResponse>();
            if (value.search == null)
            {
                total = await _context.MaintenanceReports.Where(a => a.IsDelete == false).ToListAsync();
                maintenanceReports = await _context.MaintenanceReports.Where(a => a.IsDelete == false).Select(a => new MaintenanceReportResponse
                {
                    id = a.Id,
                    name = a.Name,
                    code = a.Code,
                    update_date = a.UpdateDate,
                    create_date = a.CreateDate,
                    description = a.Description,
                    is_delete = a.IsDelete,
                    status = a.Status,
                    agency = new AgencyViewResponse
                    {
                        id = a.AgencyId,
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Telephone).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = a.CustomerId,
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                    },
                    service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(a.Id)).Select(a => new ServiceViewResponse
                    {
                        id = a.ServiceId,
                        code = a.Service!.Code,
                        service_name = a.Service!.ServiceName,
                        description = a.Service!.Description
                    }).ToList(),
                    maintenance_schedule = new MaintenanceReportViewResponse
                    {
                        id = a.MaintenanceScheduleId,
                        code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                    },
                    create_by = new TechnicianViewResponse
                    {
                        id = a.CreateBy,
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {

                total = await _context.MaintenanceReports.Where(a => a.IsDelete == false
                && (a.Status!.Contains(value.search)
                || a.Name!.Contains(value.search)
                || a.Code!.Contains(value.search))).ToListAsync();
                maintenanceReports = await _context.MaintenanceReports.Where(a => a.IsDelete == false
                && (a.Status!.Contains(value.search)
                || a.Name!.Contains(value.search)
                || a.Code!.Contains(value.search))).Select(a => new MaintenanceReportResponse
                {
                    id = a.Id,
                    name = a.Name,
                    code = a.Code,
                    update_date = a.UpdateDate,
                    create_date = a.CreateDate,
                    description = a.Description,
                    is_delete = a.IsDelete,
                    status = a.Status,
                    agency = new AgencyViewResponse
                    {
                        id = a.AgencyId,
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Telephone).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = a.CustomerId,
                        code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                    },
                    service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(a.Id)).Select(a => new ServiceViewResponse
                    {
                        id = a.ServiceId,
                        code = a.Service!.Code,
                        service_name = a.Service!.ServiceName,
                        description = a.Service!.Description
                    }).ToList(),
                    maintenance_schedule = new MaintenanceReportViewResponse
                    {
                        id = a.MaintenanceScheduleId,
                        code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                    },
                    create_by = new TechnicianViewResponse
                    {
                        id = a.CreateBy,
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            return new ResponseModel<MaintenanceReportResponse>(maintenanceReports)
            {
                Total = total.Count,
                Type = "MaintenanceReports"
            };

        }
        public async Task<ObjectModelResponse> GetDetailsMaintenanceReport(Guid id)
        {
            var maintenanceReports = new MaintenanceReportResponse();
            maintenanceReports = await _context.MaintenanceReports.Where(a => a.IsDelete == false && a.Id.Equals(id)).Select(a => new MaintenanceReportResponse
            {
                id = a.Id,
                name = a.Name,
                code = a.Code,
                update_date = a.UpdateDate,
                create_date = a.CreateDate,
                description = a.Description,
                is_delete = a.IsDelete,
                status = a.Status,
                agency = new AgencyViewResponse
                {
                    id = a.AgencyId,
                    code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                    agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                    phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Telephone).FirstOrDefault(),
                    address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                },
                customer = new CustomerViewResponse
                {
                    id = a.CustomerId,
                    code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Code).FirstOrDefault(),
                    name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                    description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                },

                maintenance_schedule = new MaintenanceReportViewResponse
                {
                    id = a.MaintenanceScheduleId,
                    code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                    name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                },
                create_by = new TechnicianViewResponse
                {
                    id = a.CreateBy,
                    code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                    name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                },
                service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(a.Id)).Select(a => new ServiceViewResponse
                {
                    id = a.ServiceId,
                    code = a.Service!.Code,
                    service_name = a.Service!.ServiceName,
                    description = a.Service!.Description
                }).ToList(),
            }).FirstOrDefaultAsync();

            return new ObjectModelResponse(maintenanceReports!)
            {
                Type = "MaintenanceReport"
            };

        }
        public async Task<ObjectModelResponse> CreateMaintenanceReport(MaintenanceReportRequest model)
        {
            var maintenanceReport_id = Guid.NewGuid();
            while (true)
            {
                var maintenanceReport_dup = await _context.MaintenanceReports.Where(x => x.Id.Equals(maintenanceReport_id)).FirstOrDefaultAsync();
                if (maintenanceReport_dup == null)
                {
                    break;
                }
                else
                {
                    maintenanceReport_id = Guid.NewGuid();
                }
            }
            var agencyId = _context.MaintenanceSchedules.Where(a => a.Id.Equals(model.maintenance_schedule_id)).Select(a => a.AgencyId).FirstOrDefault();
            var num = await GetLastCode();
            var code = CodeHelper.GeneratorCode("MR", num + 1);
            var maintenanceReport = new MaintenanceReport
            {
                Id = maintenanceReport_id,
                Code = code,
                Name = model.name,
                IsDelete = false,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7),
                Description = model.description,
                AgencyId = agencyId,
                CustomerId = _context.Agencies.Where(a => a.Id.Equals(agencyId)).Select(a => a.CustomerId).FirstOrDefault(),
                CreateBy = _context.MaintenanceSchedules.Where(a => a.Id.Equals(model.maintenance_schedule_id)).Select(a => a.TechnicianId).FirstOrDefault(),
                MaintenanceScheduleId = model.maintenance_schedule_id,
                Status = ReportStatus.NO_PROBLEM.ToString(),
            };
            var maintenanceScheduleStatus = await _context.MaintenanceSchedules.Where(a => a.Id.Equals(model.maintenance_schedule_id)).FirstOrDefaultAsync();
            maintenanceScheduleStatus!.Status = ScheduleStatus.COMPLETED.ToString();
            var data = new MaintenanceReportResponse();

            await _context.MaintenanceReports.AddAsync(maintenanceReport);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new MaintenanceReportResponse

                {
                    id = maintenanceReport.Id,
                    name = maintenanceReport.Name,
                    code = maintenanceReport.Code,
                    update_date = maintenanceReport.UpdateDate,
                    create_date = maintenanceReport.CreateDate,
                    description = maintenanceReport.Description,
                    is_delete = maintenanceReport.IsDelete,
                    status = maintenanceReport.Status,
                    agency = new AgencyViewResponse
                    {
                        id = maintenanceReport.AgencyId,
                        code = _context.Agencies.Where(x => x.Id.Equals(maintenanceReport.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(maintenanceReport.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(maintenanceReport.AgencyId)).Select(a => a.Telephone).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(maintenanceReport.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = maintenanceReport.CustomerId,
                        code = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                    },

                    maintenance_schedule = new MaintenanceReportViewResponse
                    {
                        id = maintenanceReport.MaintenanceScheduleId,
                        code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                    },
                    create_by = new TechnicianViewResponse
                    {
                        id = maintenanceReport.CreateBy,
                        code = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(maintenanceReport.Id)).Select(a => new ServiceViewResponse
                    {
                        id = a.ServiceId,
                        code = a.Service!.Code,
                        service_name = a.Service!.ServiceName,
                        description = a.Service!.Description
                    }).ToList(),
                };
            }
            return new ObjectModelResponse(data)
            {

                Type = "MaintenanceReport"
            };
        }
        public async Task<ResponseModel<MaintenanceReportServiceResponse>> CreateMaintenanceReportService(Guid id, ListMaintenanceReportServiceRequest model)
        {
            var maintenanceReport = await _context.MaintenanceReports.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            var technician = await _context.Technicians.Where(x => x.Id.Equals(maintenanceReport!.CreateBy)).FirstOrDefaultAsync();
            maintenanceReport!.Status = ReportStatus.PROBLEM.ToString();
            technician!.IsBusy = false;
            _context.MaintenanceReports.Update(maintenanceReport);
            _context.Technicians.Update(technician);
            var list = new List<MaintenanceReportServiceResponse>();
            foreach (var item in model.maintenance_report_service)
            {
                var maintenanceReportService_id = Guid.NewGuid();
                while (true)
                {
                    var maintenanceReportService_dup = await _context.MaintenanceReportServices.Where(x => x.Id.Equals(maintenanceReportService_id)).FirstOrDefaultAsync();
                    if (maintenanceReportService_dup == null)
                    {
                        break;
                    }
                    else
                    {
                        maintenanceReportService_id = Guid.NewGuid();
                    }
                }
                var maintenanceReportService = new MaintenanceReportService
                {
                    Id = maintenanceReportService_id,
                    Description = item.Description,
                    MaintenanceReportId = maintenanceReport.Id,
                    ServiceId = item.service_id,
                };
                await _context.MaintenanceReportServices.AddAsync(maintenanceReportService);
                list.Add(new MaintenanceReportServiceResponse
                {
                    id = maintenanceReportService.Id,
                    maintenance_report_id = maintenanceReportService.MaintenanceReportId,
                    service_id = maintenanceReportService.ServiceId,
                    description = maintenanceReportService.Description
                });
                await _context.SaveChangesAsync();
            }
            return new ResponseModel<MaintenanceReportServiceResponse>(list!)
            {
                Total = list.Count,
                Type = "MaintenanceReportServices"
            };

        }

        private async Task<int> GetLastCode()
        {
            var maintenanceReport = await _context.MaintenanceReports.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(maintenanceReport!.Code!);
        }
    }
}
