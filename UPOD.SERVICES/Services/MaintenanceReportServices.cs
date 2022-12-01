using Microsoft.EntityFrameworkCore;
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
        Task<ResponseModel<MaintenanceReportResponse>> GetListMaintenanceReports(PaginationRequest model, FilterStatusRequest value);
        Task<ObjectModelResponse> CreateMaintenanceReport(MaintenanceReportRequest model);
        Task<ObjectModelResponse> GetDetailsMaintenanceReport(Guid id);
        Task<ObjectModelResponse> SetStatusProcessingMaintenanceReport(Guid id);
        Task<ObjectModelResponse> SetUnProcessingMaintenanceReport(Guid id);
        Task<ObjectModelResponse> UpdateMaintenanceReport(Guid id, MaintenanceReportRequest model);
        Task SetMaintenanceReportStatus();
        Task SetMaintenanceReportStatusProcessing();
    }

    public class MaintenanceReportServices : IMaintenanceReportService
    {
        private readonly Database_UPODContext _context;
        public MaintenanceReportServices(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task SetMaintenanceReportStatus()
        {
            var report_services = await _context.MaintenanceReports.Where(a => a.CreateDate!.Value.AddDays(2).Date <= DateTime.UtcNow.AddHours(7).Date).ToListAsync();
            foreach (var item in report_services)
            {
                if (item.Status!.Equals("STABILIZED"))
                {
                    //item.UpdateDate = DateTime.UtcNow.AddHours(7);
                    item.Status = ReportStatus.CLOSED.ToString();
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task SetMaintenanceReportStatusProcessing()
        {
            var maintenance_reports = await _context.MaintenanceReports.Where(a => a.Status!.Equals("PROCESSING")).ToListAsync();
            foreach (var item in maintenance_reports)
            {
                int count = 0;
                var date = DateTime.UtcNow.AddHours(7);
                var report_services = await _context.MaintenanceReportServices.Where(a => a.MaintenanceReportId.Equals(item.Id)).ToListAsync();
                foreach (var item1 in report_services)
                {
                    if (item1.Created == true)
                    {
                        count = count + 1;
                    }
                    date = item.CreateDate!.Value.AddDays(2);
                }
                if (report_services.Count == count)
                {
                    if (date.Date <= DateTime.UtcNow.AddHours(7).Date)
                    {
                        //item.UpdateDate = DateTime.UtcNow.AddHours(7);
                        item.Status = ReportStatus.CLOSED.ToString();
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task<ObjectModelResponse> SetUnProcessingMaintenanceReport(Guid id)
        {
            var status = await _context.MaintenanceReports.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            status!.Status = ReportStatus.TROUBLED.ToString();
            status!.UpdateDate = DateTime.UtcNow.AddHours(7);
            var report_services = await _context.MaintenanceReportServices.Where(a => a.MaintenanceReportId.Equals(id)).ToListAsync();
            foreach (var item in report_services)
            {
                var request = await _context.Requests.Where(a => a.Id.Equals(item.RequestId)).FirstOrDefaultAsync();
                if (request != null)
                {
                    item.Created = false;
                    item.RequestId = null;
                    request!.UpdateDate = DateTime.UtcNow.AddHours(7);
                    request!.RequestStatus = ProcessStatus.CANCELED.ToString();
                }
            }
            var rs = await _context.SaveChangesAsync();
            var data = new MaintenanceReportResponse();
            if (rs > 0)
            {
                data = new MaintenanceReportResponse
                {
                    id = status.Id,
                    name = status.Name,
                    code = status.Code,
                    update_date = status.UpdateDate,
                    create_date = status.CreateDate,
                    description = status.Description,
                    is_delete = status.IsDelete,
                    status = status.Status,
                    agency = new AgencyViewResponse
                    {
                        id = status.AgencyId,
                        code = _context.Agencies.Where(x => x.Id.Equals(status.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(status.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(status.AgencyId)).Select(a => a.Telephone).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(status.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = status.CustomerId,
                        code = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Code).FirstOrDefault(),
                        cus_name = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Phone).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                    },

                    maintenance_schedule = new MaintenanceReportViewResponse
                    {
                        id = status.MaintenanceScheduleId,
                        code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(status.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.MaintenanceSchedules.Where(x => x.Id.Equals(status.MaintenanceScheduleId)).Select(a => a.Description).FirstOrDefault(),
                        maintain_time = _context.MaintenanceSchedules.Where(x => x.Id.Equals(status.MaintenanceScheduleId)).Select(a => a.MaintainTime).FirstOrDefault(),
                        sche_name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(status.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                    },
                    create_by = new TechnicianViewResponse
                    {
                        id = status.CreateBy,
                        code = _context.Technicians.Where(x => x.Id.Equals(status.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(status.CreateBy)).Select(a => a.Telephone).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(status.CreateBy)).Select(a => a.Email).FirstOrDefault(),
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(status.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(status.Id)).Select(a => new ServiceReportResponse
                    {
                        report_service_id = a.Id,
                        service_id = a.ServiceId,
                        code = a.Service!.Code,
                        service_name = a.Service!.ServiceName,
                        description = a.Description,
                        created = a.Created,
                        request_id = a.RequestId,
                    }).ToList(),
                };
            }
            return new ObjectModelResponse(data)
            {
                Type = "MaintenanceReport",
            };
        }
        public async Task<ObjectModelResponse> SetStatusProcessingMaintenanceReport(Guid id)
        {
            var status = await _context.MaintenanceReports.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            status!.Status = ReportStatus.PROCESSING.ToString();
            status!.UpdateDate = DateTime.UtcNow.AddHours(7);
            var rs = await _context.SaveChangesAsync();
            var data = new MaintenanceReportResponse();
            if (rs > 0)
            {
                data = new MaintenanceReportResponse
                {
                    id = status.Id,
                    name = status.Name,
                    code = status.Code,
                    update_date = status.UpdateDate,
                    create_date = status.CreateDate,
                    description = status.Description,
                    is_delete = status.IsDelete,
                    status = status.Status,
                    agency = new AgencyViewResponse
                    {
                        id = status.AgencyId,
                        code = _context.Agencies.Where(x => x.Id.Equals(status.AgencyId)).Select(a => a.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(status.AgencyId)).Select(a => a.AgencyName).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(status.AgencyId)).Select(a => a.Telephone).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(status.AgencyId)).Select(a => a.Address).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = status.CustomerId,
                        code = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Code).FirstOrDefault(),
                        cus_name = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Phone).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(status.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                    },

                    maintenance_schedule = new MaintenanceReportViewResponse
                    {
                        id = status.MaintenanceScheduleId,
                        code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(status.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.MaintenanceSchedules.Where(x => x.Id.Equals(status.MaintenanceScheduleId)).Select(a => a.Description).FirstOrDefault(),
                        maintain_time = _context.MaintenanceSchedules.Where(x => x.Id.Equals(status.MaintenanceScheduleId)).Select(a => a.MaintainTime).FirstOrDefault(),
                        sche_name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(status.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                    },
                    create_by = new TechnicianViewResponse
                    {
                        id = status.CreateBy,
                        code = _context.Technicians.Where(x => x.Id.Equals(status.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(status.CreateBy)).Select(a => a.Telephone).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(status.CreateBy)).Select(a => a.Email).FirstOrDefault(),
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(status.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(status.Id)).Select(a => new ServiceReportResponse
                    {
                        report_service_id = a.Id,
                        service_id = a.ServiceId,
                        code = a.Service!.Code,
                        service_name = a.Service!.ServiceName,
                        description = a.Description,
                        created = a.Created,
                        request_id = a.RequestId,
                    }).ToList(),
                };
            }
            return new ObjectModelResponse(data)
            {
                Type = "MaintenanceReport",
            };
        }

        public async Task<ResponseModel<MaintenanceReportResponse>> GetListMaintenanceReports(PaginationRequest model, FilterStatusRequest value)
        {
            var total = await _context.MaintenanceReports.Where(a => a.IsDelete == false).ToListAsync();
            var maintenanceReports = new List<MaintenanceReportResponse>();
            if (value.search == null && value.status == null)
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
                        cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Phone).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                    },

                    maintenance_schedule = new MaintenanceReportViewResponse
                    {
                        id = a.MaintenanceScheduleId,
                        code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Description).FirstOrDefault(),
                        maintain_time = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.MaintainTime).FirstOrDefault(),
                        sche_name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                    },
                    create_by = new TechnicianViewResponse
                    {
                        id = a.CreateBy,
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Telephone).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Email).FirstOrDefault(),
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(a.Id)).Select(a => new ServiceReportResponse
                    {
                        report_service_id = a.Id,
                        service_id = a.ServiceId,
                        code = a.Service!.Code,
                        service_name = a.Service!.ServiceName,
                        description = a.Description,
                        created = a.Created,
                        request_id = a.RequestId,
                    }).ToList(),
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
                var customer_name = await _context.Customers.Where(a => a.Name!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                var contract_name = await _context.Contracts.Where(a => a.ContractName!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                var agency_name = await _context.Agencies.Where(a => a.AgencyName!.Contains(value.search!)).Select(a => a.Id).FirstOrDefaultAsync();
                total = await _context.MaintenanceReports.Where(a => a.IsDelete == false
                && (a.Status!.Contains(value.status!)
                && (a.Name!.Contains(value.search!)
                || a.AgencyId!.Equals(agency_name)
                || a.CustomerId!.Equals(customer_name)
                || a.Code!.Contains(value.search!)))).ToListAsync();
                maintenanceReports = await _context.MaintenanceReports.Where(a => a.IsDelete == false
                && (a.Status!.Contains(value.status!)
                && (a.Name!.Contains(value.search!)
                || a.AgencyId!.Equals(agency_name)
                || a.CustomerId!.Equals(customer_name)
                || a.Code!.Contains(value.search!)))).Select(a => new MaintenanceReportResponse
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
                        cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Phone).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                    },

                    maintenance_schedule = new MaintenanceReportViewResponse
                    {
                        id = a.MaintenanceScheduleId,
                        code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Description).FirstOrDefault(),
                        maintain_time = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.MaintainTime).FirstOrDefault(),
                        sche_name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                    },
                    create_by = new TechnicianViewResponse
                    {
                        id = a.CreateBy,
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Telephone).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Email).FirstOrDefault(),
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(a.Id)).Select(a => new ServiceReportResponse
                    {
                        report_service_id = a.Id,
                        service_id = a.ServiceId,
                        code = a.Service!.Code,
                        service_name = a.Service!.ServiceName,
                        description = a.Description,
                        created = a.Created,
                        request_id = a.RequestId,
                    }).ToList(),
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
            maintenanceReports = await _context.MaintenanceReports.Where(a => a.IsDelete == false && a.MaintenanceScheduleId.Equals(id)).Select(a => new MaintenanceReportResponse
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
                    cus_name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                    address = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Address).FirstOrDefault(),
                    mail = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Mail).FirstOrDefault(),
                    phone = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Phone).FirstOrDefault(),
                    description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                },

                maintenance_schedule = new MaintenanceReportViewResponse
                {
                    id = a.MaintenanceScheduleId,
                    code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                    description = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Description).FirstOrDefault(),
                    maintain_time = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.MaintainTime).FirstOrDefault(),
                    sche_name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(a.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                },
                create_by = new TechnicianViewResponse
                {
                    id = a.CreateBy,
                    code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                    phone = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Telephone).FirstOrDefault(),
                    email = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.Email).FirstOrDefault(),
                    tech_name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                },
                service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(a.Id)).Select(a => new ServiceReportResponse
                {
                    report_service_id = a.Id,
                    service_id = a.ServiceId,
                    code = a.Service!.Code,
                    service_name = a.Service!.ServiceName,
                    description = a.Description,
                    created = a.Created,
                    request_id = a.RequestId,
                }).ToList(),
            }).FirstOrDefaultAsync();

            return new ObjectModelResponse(maintenanceReports!)
            {
                Type = "MaintenanceReport"
            };

        }
        public async Task<ObjectModelResponse> UpdateMaintenanceReport(Guid id, MaintenanceReportRequest model)
        {
            var maintenanceReport = await _context.MaintenanceReports.Where(a => a.Id.Equals(id) && a.IsDelete == false).FirstOrDefaultAsync();
            maintenanceReport!.Name = model.name;
            maintenanceReport!.MaintenanceScheduleId = model.maintenance_schedule_id;
            maintenanceReport!.Description = model.description;
            maintenanceReport!.UpdateDate = DateTime.UtcNow.AddHours(7);
            maintenanceReport!.Status = ReportStatus.STABILIZED.ToString();
            if (model.service.Count == 0)
            {
                var report_service_removes = await _context.MaintenanceReportServices.Where(a => a.MaintenanceReportId.Equals(maintenanceReport.Id)).ToListAsync();
                foreach (var report_service in report_service_removes)
                {
                    _context.MaintenanceReportServices.Remove(report_service);
                }
            }
            else
            {
                maintenanceReport!.Status = ReportStatus.TROUBLED.ToString();
                var report_service_removes = await _context.MaintenanceReportServices.Where(a => a.MaintenanceReportId.Equals(maintenanceReport.Id)).ToListAsync();
                foreach (var item in report_service_removes)
                {
                    _context.MaintenanceReportServices.Remove(item);
                }
                foreach (var item1 in model.service)
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
                        Description = item1.Description,
                        MaintenanceReportId = maintenanceReport.Id,
                        ServiceId = item1.service_id,
                        Created = false,
                        RequestId = null,
                    };
                    await _context.MaintenanceReportServices.AddAsync(maintenanceReportService);

                }
            }
            var data = new MaintenanceReportResponse();
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
                        cus_name = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Phone).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                    },

                    maintenance_schedule = new MaintenanceReportViewResponse
                    {
                        id = maintenanceReport.MaintenanceScheduleId,
                        code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.Description).FirstOrDefault(),
                        maintain_time = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.MaintainTime).FirstOrDefault(),
                        sche_name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                    },
                    create_by = new TechnicianViewResponse
                    {
                        id = maintenanceReport.CreateBy,
                        code = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.Telephone).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.Email).FirstOrDefault(),
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(maintenanceReport.Id)).Select(a => new ServiceReportResponse
                    {
                        report_service_id = a.Id,
                        service_id = a.ServiceId,
                        code = a.Service!.Code,
                        service_name = a.Service!.ServiceName,
                        description = a.Description,
                        created = a.Created,
                        request_id = a.RequestId,
                    }).ToList(),
                };
            }
            return new ObjectModelResponse(data)
            {
                Type = "MaintenanceReport"
            };
        }
        public async Task<ObjectModelResponse> CreateMaintenanceReport(MaintenanceReportRequest model)
        {
            var data = new MaintenanceReportResponse();
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
            while (true)
            {
                var code_dup = await _context.MaintenanceReports.Where(a => a.Code.Equals(code)).FirstOrDefaultAsync();
                if (code_dup == null)
                {
                    break;
                }
                else
                {
                    code = "MR-" + num++.ToString();
                }
            }
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
                Status = ReportStatus.STABILIZED.ToString(),
            };
            if (model.service.Count == 0)
            {
                var maintenanceScheduleStatus = await _context.MaintenanceSchedules.Where(a => a.Id.Equals(model.maintenance_schedule_id)).FirstOrDefaultAsync();
                maintenanceScheduleStatus!.Status = ScheduleStatus.COMPLETED.ToString();
                maintenanceScheduleStatus!.EndDate = DateTime.UtcNow.AddHours(7);
                var technician = await _context.Technicians.Where(x => x.Id.Equals(maintenanceReport!.CreateBy)).FirstOrDefaultAsync();
                await _context.MaintenanceReports.AddAsync(maintenanceReport);
                technician!.IsBusy = false;
                _context.Technicians.Update(technician);
            }
            else
            {
                var maintenanceScheduleStatus = await _context.MaintenanceSchedules.Where(a => a.Id.Equals(model.maintenance_schedule_id)).FirstOrDefaultAsync();
                maintenanceScheduleStatus!.Status = ScheduleStatus.COMPLETED.ToString();
                maintenanceScheduleStatus!.EndDate = DateTime.UtcNow.AddHours(7);
                await _context.MaintenanceReports.AddAsync(maintenanceReport);
                var technician = await _context.Technicians.Where(x => x.Id.Equals(maintenanceReport!.CreateBy)).FirstOrDefaultAsync();
                maintenanceReport!.Status = ReportStatus.TROUBLED.ToString();
                technician!.IsBusy = false;
                _context.Technicians.Update(technician);
                foreach (var item in model.service)
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
                        Created = false,
                        RequestId = null,
                    };
                    await _context.MaintenanceReportServices.AddAsync(maintenanceReportService);
                }
            }

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
                        cus_name = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Name).FirstOrDefault(),
                        address = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Address).FirstOrDefault(),
                        mail = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Mail).FirstOrDefault(),
                        phone = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Phone).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(maintenanceReport.CustomerId)).Select(a => a.Description).FirstOrDefault(),
                    },

                    maintenance_schedule = new MaintenanceReportViewResponse
                    {
                        id = maintenanceReport.MaintenanceScheduleId,
                        code = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.Code).FirstOrDefault(),
                        description = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.Description).FirstOrDefault(),
                        maintain_time = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.MaintainTime).FirstOrDefault(),
                        sche_name = _context.MaintenanceSchedules.Where(x => x.Id.Equals(maintenanceReport.MaintenanceScheduleId)).Select(a => a.Name).FirstOrDefault(),
                    },
                    create_by = new TechnicianViewResponse
                    {
                        id = maintenanceReport.CreateBy,
                        code = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.Code).FirstOrDefault(),
                        phone = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.Telephone).FirstOrDefault(),
                        email = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.Email).FirstOrDefault(),
                        tech_name = _context.Technicians.Where(x => x.Id.Equals(maintenanceReport.CreateBy)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    service = _context.MaintenanceReportServices.Where(x => x.MaintenanceReportId.Equals(maintenanceReport.Id)).Select(a => new ServiceReportResponse
                    {
                        report_service_id = a.Id,
                        service_id = a.ServiceId,
                        code = a.Service!.Code,
                        service_name = a.Service!.ServiceName,
                        description = a.Description,
                        created = a.Created,
                        request_id = a.RequestId,
                    }).ToList(),
                };
            }
            return new ObjectModelResponse(data)
            {
                Type = "MaintenanceReport"
            };
        }


        private async Task<int> GetLastCode()
        {
            var maintenanceReport = await _context.MaintenanceReports.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(maintenanceReport!.Code!);
        }
    }
}
