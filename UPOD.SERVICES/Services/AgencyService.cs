using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.REPOSITORIES.Services;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{

    public interface IAgencyService
    {
        Task<ResponseModel<AgencyResponse>> GetListAgencies(PaginationRequest model);
        Task<ObjectModelResponse> GetDetailsAgency(Guid id);
        Task<ObjectModelResponse> CreateAgency(AgencyRequest model);
        Task<ObjectModelResponse> UpdateAgency(Guid id, AgencyUpdateRequest model);
        Task<ObjectModelResponse> DisableAgency(Guid id);
    }

    public class AgencyService : IAgencyService
    {
        private readonly Database_UPODContext _context;
        public AgencyService(Database_UPODContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<AgencyResponse>> GetListAgencies(PaginationRequest model)
        {
            var agencies = await _context.Agencies.Where(a => a.IsDelete == false).Include(c => c.Customer).Include(a => a.Area).Select(a => new AgencyResponse

            {
                id = a.Id,
                code = a.Code,
                customer = new CustomerViewResponse
                {
                    id = a.CustomerId,
                    code = a.Customer!.Code,
                    name = a.Customer.Name,
                    description = a.Customer.Description,
                    percent_for_technican_exp = a.Customer.PercentForTechnicianExp,
                    percent_for_technican_familiar_with_agency = a.Customer.PercentForTechnicianFamiliarWithAgency,
                    percent_for_technican_rate = a.Customer.PercentForTechnicianRate,
                },
                area = new AreaViewResponse
                {
                    id = a.AreaId,
                    code = a.Area!.Code,
                    area_name = a.Area.AreaName,
                    description = a.Area.Description
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
            return new ResponseModel<AgencyResponse>(agencies)
            {
                Total = agencies.Count,
                Type = "Agencies"
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
                    percent_for_technican_exp = a.Customer.PercentForTechnicianExp,
                    percent_for_technican_familiar_with_agency = a.Customer.PercentForTechnicianFamiliarWithAgency,
                    percent_for_technican_rate = a.Customer.PercentForTechnicianRate,
                },
                area = new AreaViewResponse
                {
                    id = a.AreaId,
                    code = a.Area!.Code,
                    area_name = a.Area.AreaName,
                    description = a.Area.Description
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
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("AG", code_number + 1);
            var agency = new Agency
            {
                Id = Guid.NewGuid(),
                Code = code,
                CustomerId = model.customer_id,
                AgencyName = model.agency_name,
                AreaId = model.area_id,
                ManagerName = model.manager_name,
                Address = model.address,
                Telephone = model.telephone,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
            var data = new AgencyResponse();
            var message = "blank";
            var status = 500;
            var agency_id = await _context.Agencies.Where(x => x.Id.Equals(agency.Id)).FirstOrDefaultAsync();
            if (agency_id != null)
            {
                status = 400;
                message = "AgencyId is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Agencies.AddAsync(agency);
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    data = new AgencyResponse
                    {
                        id = agency.Id,
                        code = agency.Code,
                        customer =  new CustomerViewResponse
                        {
                            id = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x=>x.Id).FirstOrDefault(),
                            code = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                            name = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                            description = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                            percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                            percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                            percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
                        },
                        area = new AreaViewResponse
                        {
                            id = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Id).FirstOrDefault(),
                            code = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Code).FirstOrDefault(),
                            area_name = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                            description = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Description).FirstOrDefault(),
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
            }
            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
                Type = "Agency"
            };
        }


        public async Task<ObjectModelResponse> DisableAgency(Guid id)
        {
            var agency = await _context.Agencies.Where(x => x.Id.Equals(id)).Include(x => x.Customer).Include(x => x.Area).FirstOrDefaultAsync();
            agency!.IsDelete = true;
            agency.UpdateDate = DateTime.Now;
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
                        percent_for_technican_exp = agency.Customer.PercentForTechnicianExp,
                        percent_for_technican_familiar_with_agency = agency.Customer.PercentForTechnicianFamiliarWithAgency,
                        percent_for_technican_rate = agency.Customer.PercentForTechnicianRate,
                    },
                    area = new AreaViewResponse
                    {
                        id = agency.AreaId,
                        code = agency.Area!.Code,
                        area_name = agency.Area.AreaName,
                        description = agency.Area.Description
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
                AgencyName = model.agency_name,
                AreaId = model.area_id,
                ManagerName = model.manager_name,
                Address = model.address,
                Telephone = model.telephone,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now
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
                        percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                        percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                        percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(agency.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
                    },
                    area = new AreaViewResponse
                    {
                        id = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Code).FirstOrDefault(),
                        area_name = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.AreaName).FirstOrDefault(),
                        description = _context.Areas.Where(x => x.Id.Equals(agency.AreaId)).Select(x => x.Description).FirstOrDefault(),
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
