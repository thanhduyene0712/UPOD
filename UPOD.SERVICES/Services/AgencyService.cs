using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.Services;

namespace UPOD.SERVICES.Services
{

    public interface IAgencyService
    {
        Task<ResponseModel<AgencyResponse>> GetListAgencies(PaginationRequest model);
        Task<ResponseModel<AgencyResponse>> GetDetailAgency(Guid id);
        Task<ResponseModel<AgencyResponse>> CreateAgency(AgencyRequest model);
        Task<ResponseModel<AgencyResponse>> UpdateAgency(Guid id, AgencyUpdateRequest model);
        Task<ResponseModel<AgencyResponse>> DisableAgency(Guid id);
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
            var agencies = await _context.Agencies.Where(a => a.IsDelete == false).Select(a => new AgencyResponse
            {
                id = a.Id,
                company_id = a.CompanyId,
                area_id = a.AreaId,
                manager_name = a.ManagerName,
                agency_name = a.AgencyName,
                address = a.Address,
                telephone = a.Telephone,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
                device_id = _context.AgencyDevices.Where(x => x.AgencyId.Equals(a.Id)).Select(x => x.DeviceId).ToList(),
            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AgencyResponse>(agencies)
            {
                Total = agencies.Count,
                Type = "Agencies"
            };
        }
        public async Task<ResponseModel<AgencyResponse>> GetDetailAgency(Guid id)
        {
            var agency = await _context.Agencies.Where(a => a.IsDelete == false && a.Id.Equals(id)).Select(a => new AgencyResponse
            {
                id = a.Id,
                company_id = a.CompanyId,
                area_id = a.AreaId,
                manager_name = a.ManagerName,
                agency_name = a.AgencyName,
                address = a.Address,
                telephone = a.Telephone,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
                device_id = _context.AgencyDevices.Where(x => x.AgencyId.Equals(a.Id)).Select(x => x.DeviceId).ToList(),

            }).ToListAsync();
            return new ResponseModel<AgencyResponse>(agency)
            {
                Total = agency.Count,
                Type = "Agency"
            };
        }


        public async Task<ResponseModel<AgencyResponse>> CreateAgency(AgencyRequest model)
        {

            var agency = new Agency
            {
                Id = Guid.NewGuid(),
                CompanyId = model.company_id,
                AgencyName = model.agency_name,
                AreaId = model.area_id,
                ManagerName = model.manager_name,
                Address = model.address,
                Telephone = model.telephone,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
            foreach (var item in model.device_id)
            {
                var agency_device = new AgencyDevice
                {
                    DeviceId = item,
                    AgencyId = agency.Id
                };
                _context.AgencyDevices.Add(agency_device);
            }
            var list = new List<AgencyResponse>();
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
                await _context.SaveChangesAsync();
                list.Add(new AgencyResponse
                {
                    id = agency.Id,
                    company_id = agency.CompanyId,
                    area_id = agency.AreaId,
                    manager_name = agency.ManagerName,
                    agency_name = agency.AgencyName,
                    address = agency.Address,
                    telephone = agency.Telephone,
                    is_delete = agency.IsDelete,
                    create_date = agency.CreateDate,
                    update_date = agency.UpdateDate,
                    device_id = _context.AgencyDevices.Where(x => x.AgencyId.Equals(agency.Id)).Select(x => x.DeviceId).ToList(),

                });
            }

            return new ResponseModel<AgencyResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Agency"
            };
        }


        public async Task<ResponseModel<AgencyResponse>> DisableAgency(Guid id)
        {
            var agency = await _context.Agencies.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            agency.IsDelete = true;
            agency.UpdateDate = DateTime.Now;
            _context.Agencies.Update(agency);
            await _context.SaveChangesAsync();
            var list = new List<AgencyResponse>();
            list.Add(new AgencyResponse
            {
                is_delete = agency.IsDelete,
            });
            return new ResponseModel<AgencyResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Agency"
            };
        }
        public async Task<ResponseModel<AgencyResponse>> UpdateAgency(Guid id, AgencyUpdateRequest model)
        {
            var agency = await _context.Agencies.Where(a => a.Id.Equals(id)).Select(x => new Agency
            {
                Id = id,
                CompanyId = x.CompanyId,
                AgencyName = model.agency_name,
                AreaId = model.area_id,
                ManagerName = model.manager_name,
                Address = model.address,
                Telephone = model.telephone,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now
            }).FirstOrDefaultAsync();
            var agency_device_remove = await _context.AgencyDevices.Where(a => a.AgencyId.Equals(id)).ToListAsync();
            foreach (var item in agency_device_remove)
            {
                _context.AgencyDevices.Remove(item);
            }
            foreach (var item in model.device_id)
            {
                var agency_device = new AgencyDevice
                {
                    DeviceId = item,
                    AgencyId = agency.Id
                };
                _context.AgencyDevices.Add(agency_device);
            }
            _context.Agencies.Update(agency);
            await _context.SaveChangesAsync();
            var list = new List<AgencyResponse>();
            list.Add(new AgencyResponse
            {
                id = agency.Id,
                company_id = agency.CompanyId,
                area_id = agency.AreaId,
                manager_name = agency.ManagerName,
                agency_name = agency.AgencyName,
                address = agency.Address,
                telephone = agency.Telephone,
                is_delete = agency.IsDelete,
                create_date = agency.CreateDate,
                update_date = agency.UpdateDate,
                device_id = _context.AgencyDevices.Where(x => x.AgencyId.Equals(agency.Id)).Select(x => x.DeviceId).ToList(),
            });
            return new ResponseModel<AgencyResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Agency"
            };
        }

    }
}
