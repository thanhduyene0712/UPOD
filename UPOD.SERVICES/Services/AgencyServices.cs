using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{

    public interface IAgencyService
    {
        Task<ResponseModel<AgencyResponse>> GetListAgencies(PaginationRequest model, FilterRequest value);
        Task<ResponseModel<AgencyResponse>> GetListAgenciesByTechnician(PaginationRequest model, Guid id, FilterRequest value);
        Task<ObjectModelResponse> GetDetailsAgency(Guid id);
        Task<ObjectModelResponse> CreateAgency(AgencyRequest model);
        Task<ObjectModelResponse> UpdateAgency(Guid id, AgencyUpdateRequest model);
        Task<ObjectModelResponse> DisableAgency(Guid id);
        Task<ResponseModel<TechnicianViewResponse>> GetTechnicianByAgencyId(PaginationRequest model, Guid id);
    }

    public class AgencyServices : IAgencyService
    {
        private readonly Database_UPODContext _context;
        public AgencyServices(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<AgencyResponse>> GetListAgenciesByTechnician(PaginationRequest model, Guid id, FilterRequest value)
        {
            var total = await _context.Agencies.Where(a => a.IsDelete == false).ToListAsync();
            var agencies = new List<AgencyResponse>();
            if (value.search == null)
            {
                total = await _context.Agencies.Where(a => a.IsDelete == false && a.TechnicianId.Equals(id)).ToListAsync();
                agencies = await _context.Agencies.Where(a => a.IsDelete == false && a.TechnicianId.Equals(id)).Include(c => c.Customer).Include(a => a.Area).Select(a => new AgencyResponse

                {
                    id = a.Id,
                    code = a.Code,
                    customer = new CustomerViewResponse
                    {
                        id = a.CustomerId,
                        code = a.Customer!.Code,
                        name = a.Customer.Name,
                        description = a.Customer.Description,
                    },
                    area = new AreaViewResponse
                    {
                        id = a.AreaId,
                        code = a.Area!.Code,
                        area_name = a.Area.AreaName,
                        description = a.Area.Description
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = a.TechnicianId,
                        code = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    manager_name = a.ManagerName,
                    agency_name = a.AgencyName,
                    address = a.Address,
                    telephone = a.Telephone,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    device = _context.Devices.Where(x => x.AgencyId.Equals(a.Id)).Select(x => new DeviceViewResponse
                    {
                        id = x.Id,
                        code = x.Code,
                        device_name = x.DeviceName
                    }).ToList(),
                }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Agencies.Where(a => a.IsDelete == false && a.TechnicianId.Equals(id)
                && (a.Code!.Contains(value.search)
                || a.AgencyName!.Contains(value.search)
                || a.ManagerName!.Contains(value.search)
                || a.Address!.Contains(value.search)
                || a.Telephone!.Contains(value.search))).ToListAsync();
                agencies = await _context.Agencies.Where(a => a.IsDelete == false && a.TechnicianId.Equals(id)
                && (a.Code!.Contains(value.search)
                || a.AgencyName!.Contains(value.search)
                || a.ManagerName!.Contains(value.search)
                || a.Address!.Contains(value.search)
                || a.Telephone!.Contains(value.search))).Include(c => c.Customer).Include(a => a.Area).Select(a => new AgencyResponse

                {
                    id = a.Id,
                    code = a.Code,
                    customer = new CustomerViewResponse
                    {
                        id = a.CustomerId,
                        code = a.Customer!.Code,
                        name = a.Customer.Name,
                        description = a.Customer.Description,
                    },
                    area = new AreaViewResponse
                    {
                        id = a.AreaId,
                        code = a.Area!.Code,
                        area_name = a.Area.AreaName,
                        description = a.Area.Description
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = a.TechnicianId,
                        code = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    manager_name = a.ManagerName,
                    agency_name = a.AgencyName,
                    address = a.Address,
                    telephone = a.Telephone,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    device = _context.Devices.Where(x => x.AgencyId.Equals(a.Id)).Select(x => new DeviceViewResponse
                    {
                        id = x.Id,
                        code = x.Code,
                        device_name = x.DeviceName
                    }).ToList(),
                }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<AgencyResponse>(agencies)
            {
                Total = total.Count,
                Type = "Agencies"
            };
        }
        public async Task<ResponseModel<AgencyResponse>> GetListAgencies(PaginationRequest model, FilterRequest value)
        {
            var total = await _context.Agencies.Where(a => a.IsDelete == false).ToListAsync();
            var agencies = new List<AgencyResponse>();
            if (value.search == null)
            {
                total = await _context.Agencies.Where(a => a.IsDelete == false).ToListAsync();
                agencies = await _context.Agencies.Where(a => a.IsDelete == false).Include(c => c.Customer).Include(a => a.Area).Select(a => new AgencyResponse

                {
                    id = a.Id,
                    code = a.Code,
                    customer = new CustomerViewResponse
                    {
                        id = a.CustomerId,
                        code = a.Customer!.Code,
                        name = a.Customer.Name,
                        description = a.Customer.Description,
                    },
                    area = new AreaViewResponse
                    {
                        id = a.AreaId,
                        code = a.Area!.Code,
                        area_name = a.Area.AreaName,
                        description = a.Area.Description
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = a.TechnicianId,
                        code = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    manager_name = a.ManagerName,
                    agency_name = a.AgencyName,
                    address = a.Address,
                    telephone = a.Telephone,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    device = _context.Devices.Where(x => x.AgencyId.Equals(a.Id)).Select(x => new DeviceViewResponse
                    {
                        id = x.Id,
                        code = x.Code,
                        device_name = x.DeviceName
                    }).ToList(),
                }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Agencies.Where(a => a.IsDelete == false
               && (a.Code!.Contains(value.search)
               || a.AgencyName!.Contains(value.search)
               || a.ManagerName!.Contains(value.search)
               || a.Address!.Contains(value.search)
               || a.Telephone!.Contains(value.search))).ToListAsync();
                agencies = await _context.Agencies.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search)
                || a.AgencyName!.Contains(value.search)
                || a.ManagerName!.Contains(value.search)
                || a.Address!.Contains(value.search)
                || a.Telephone!.Contains(value.search))).Include(c => c.Customer).Include(a => a.Area).Select(a => new AgencyResponse

                {
                    id = a.Id,
                    code = a.Code,
                    customer = new CustomerViewResponse
                    {
                        id = a.CustomerId,
                        code = a.Customer!.Code,
                        name = a.Customer.Name,
                        description = a.Customer.Description,
                    },
                    area = new AreaViewResponse
                    {
                        id = a.AreaId,
                        code = a.Area!.Code,
                        area_name = a.Area.AreaName,
                        description = a.Area.Description
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = a.TechnicianId,
                        code = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    manager_name = a.ManagerName,
                    agency_name = a.AgencyName,
                    address = a.Address,
                    telephone = a.Telephone,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    device = _context.Devices.Where(x => x.AgencyId.Equals(a.Id)).Select(x => new DeviceViewResponse
                    {
                        id = x.Id,
                        code = x.Code,
                        device_name = x.DeviceName
                    }).ToList(),
                }).OrderByDescending(a => a.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<AgencyResponse>(agencies)
            {
                Total = total.Count,
                Type = "Agencies"
            };
        }
        public async Task<ResponseModel<TechnicianViewResponse>> GetTechnicianByAgencyId(PaginationRequest model, Guid id)
        {
            var agency = await _context.Agencies.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            var area = await _context.Areas.Where(a => a.Id.Equals(agency!.AreaId)).FirstOrDefaultAsync();
            var total = await _context.Technicians.Where(a => a.AreaId.Equals(area!.Id)).ToListAsync();
            var technician = await _context.Technicians.Where(a => a.AreaId.Equals(area!.Id)).Select(a => new TechnicianViewResponse
            {
                id = a.Id,
                code = a.Code,
                name = a.TechnicianName,
            }).ToListAsync();
            return new ResponseModel<TechnicianViewResponse>(technician)
            {
                Total = total.Count,
                Type = "Technicians"
            };
        }
        public async Task<ObjectModelResponse> GetDetailsAgency(Guid id)
        {
            var agency = await _context.Agencies.Where(a => a.IsDelete == false && a.Id.Equals(id)).Select(a => new AgencyResponse
            {
                id = a.Id,
                code = a.Code,
                customer = new CustomerViewResponse
                {
                    id = a.CustomerId,
                    code = a.Customer!.Code,
                    name = a.Customer.Name,
                    description = a.Customer.Description,
                },
                area = new AreaViewResponse
                {
                    id = a.AreaId,
                    code = a.Area!.Code,
                    area_name = a.Area.AreaName,
                    description = a.Area.Description
                },
                technician = new TechnicianViewResponse
                {
                    id = a.TechnicianId,
                    code = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                    name = _context.Technicians.Where(x => x.Id.Equals(a.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                },
                manager_name = a.ManagerName,
                agency_name = a.AgencyName,
                address = a.Address,
                telephone = a.Telephone,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
                device = _context.Devices.Where(x => x.AgencyId.Equals(a.Id)).Select(x => new DeviceViewResponse
                {
                    id = x.Id,
                    code = x.Code,
                    device_name = x.DeviceName
                }).ToList(),

            }).FirstOrDefaultAsync();
            return new ObjectModelResponse(agency!)
            {
                Type = "Agency"
            };
        }


        public async Task<ObjectModelResponse> CreateAgency(AgencyRequest model)
        {
            var agency_id = Guid.NewGuid();
            while (true)
            {
                var agency_dup = await _context.Agencies.Where(x => x.Id.Equals(agency_id)).FirstOrDefaultAsync();
                if (agency_dup == null)
                {
                    break;
                }
                else
                {
                    agency_id = Guid.NewGuid();
                }
            }
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("AG", code_number + 1);
            var agency = new Agency
            {
                Id = agency_id,
                Code = code,
                CustomerId = model.customer_id,
                AgencyName = model.agency_name,
                TechnicianId = model.technician_id,
                AreaId = model.area_id,
                ManagerName = model.manager_name,
                Address = model.address,
                Telephone = model.telephone,
                IsDelete = false,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7)

            };
            var data = new AgencyResponse();

            await _context.Agencies.AddAsync(agency);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new AgencyResponse
                {
                    id = agency.Id,
                    code = agency.Code,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Description).FirstOrDefault(),

                    },
                    area = new AreaViewResponse
                    {
                        id = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Code).FirstOrDefault(),
                        area_name = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                        description = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = agency.TechnicianId,
                        code = _context.Technicians.Where(x => x.Id.Equals(agency.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(agency.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    manager_name = agency.ManagerName,
                    agency_name = agency.AgencyName,
                    address = agency.Address,
                    telephone = agency.Telephone,
                    is_delete = agency.IsDelete,
                    create_date = agency.CreateDate,
                    update_date = agency.UpdateDate,
                    device = _context.Devices.Where(x => x.AgencyId.Equals(agency.Id)).Select(x => new DeviceViewResponse
                    {
                        id = x.Id,
                        code = x.Code,
                        device_name = x.DeviceName
                    }).ToList(),

                };

            }
            return new ObjectModelResponse(data)
            {

                Type = "Agency"
            };
        }


        public async Task<ObjectModelResponse> DisableAgency(Guid id)
        {
            var agency = await _context.Agencies.Where(x => x.Id.Equals(id)).Include(x => x.Customer).Include(x => x.Area).FirstOrDefaultAsync();
            agency!.IsDelete = true;
            agency.UpdateDate = DateTime.UtcNow.AddHours(7);
            var data = new AgencyResponse();
            _context.Agencies.Update(agency);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new AgencyResponse
                {
                    id = agency.Id,
                    code = agency.Code,
                    customer = new CustomerViewResponse
                    {
                        id = agency.CustomerId,
                        code = agency.Customer!.Code,
                        name = agency.Customer.Name,
                        description = agency.Customer.Description,
                    },
                    area = new AreaViewResponse
                    {
                        id = agency.AreaId,
                        code = agency.Area!.Code,
                        area_name = agency.Area.AreaName,
                        description = agency.Area.Description
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = agency.TechnicianId,
                        code = _context.Technicians.Where(x => x.Id.Equals(agency.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(agency.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    manager_name = agency.ManagerName,
                    agency_name = agency.AgencyName,
                    address = agency.Address,
                    telephone = agency.Telephone,
                    is_delete = agency.IsDelete,
                    create_date = agency.CreateDate,
                    update_date = agency.UpdateDate,
                    device = _context.Devices.Where(x => x.AgencyId.Equals(agency.Id)).Select(x => new DeviceViewResponse
                    {
                        id = x.Id,
                        code = x.Code,
                        device_name = x.DeviceName
                    }).ToList(),
                };
            }
            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Agency"
            };
        }
        public async Task<ObjectModelResponse> UpdateAgency(Guid id, AgencyUpdateRequest model)
        {
            var agency = await _context.Agencies.Where(a => a.Id.Equals(id)).Include(x => x.Area).Include(x => x.Customer).Include(x => x.Devices).Select(x => new Agency
            {
                Id = id,
                Code = x.Code,
                CustomerId = x.CustomerId,
                TechnicianId = model.technician_id,
                AgencyName = model.agency_name,
                AreaId = model.area_id,
                ManagerName = model.manager_name,
                Address = model.address,
                Telephone = model.telephone,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.UtcNow.AddHours(7)
            }).FirstOrDefaultAsync();

            _context.Agencies.Update(agency!);
            var rs = await _context.SaveChangesAsync();
            var data = new AgencyResponse();
            if (rs > 0)
            {
                data = new AgencyResponse
                {
                    id = agency!.Id,
                    code = agency.Code,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Description).FirstOrDefault(),

                    },
                    area = new AreaViewResponse
                    {
                        id = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Code).FirstOrDefault(),
                        area_name = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                        description = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = agency.TechnicianId,
                        code = _context.Technicians.Where(x => x.Id.Equals(agency.TechnicianId)).Select(a => a.Code).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(agency.TechnicianId)).Select(a => a.TechnicianName).FirstOrDefault(),
                    },
                    manager_name = agency.ManagerName,
                    agency_name = agency.AgencyName,
                    address = agency.Address,
                    telephone = agency.Telephone,
                    is_delete = agency.IsDelete,
                    create_date = agency.CreateDate,
                    update_date = agency.UpdateDate,
                    device = _context.Devices.Where(x => x.AgencyId.Equals(agency.Id)).Select(x => new DeviceViewResponse
                    {
                        id = x.Id,
                        code = x.Code,
                        device_name = x.DeviceName
                    }).ToList(),
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Agency"
            };
        }
        private async Task<int> GetLastCode()
        {
            var agency = await _context.Agencies.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(agency!.Code!);
        }
    }
}
