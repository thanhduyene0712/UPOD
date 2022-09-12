using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Reso.Core.BaseConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.Repositories;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.Services;

namespace UPOD.SERVICES.Services
{
    public interface IAgencyService
    {
        Task<ResponseModel<AgencyRespone>> GetAll(PaginationRequest model);
        Task<ResponseModel<AgencyRespone>> SearchAgencies(PaginationRequest model, String value);
        Task<ResponseModel<AgencyRespone>> UpdateAgency(Guid id, AgencyRequest model);
        Task<ResponseModel<AgencyRespone>> CreateAgency(AgencyRequest model);
        Task<ResponseModel<AgencyRespone>> DisableAgency(Guid id, AgencyDisableRequest model);


    }

    public class AgencyService : IAgencyService
    {
        private readonly Database_UPODContext _context;
        public AgencyService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<AgencyRespone>> GetAll(PaginationRequest model)
        {
            var agencies = await _context.Agencies.Select(a => new AgencyRespone
            {
                id = a.Id,
                agency_name = a.AgencyName,
                username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
                address = a.Address,
                company_name = _context.Companies.Where(x => x.Id.Equals(a.CompanyId)).Select(x => x.CompanyName).FirstOrDefault(),
                manager_name = a.ManagerName,
                telephone = a.Telephone,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,


            }).OrderBy(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AgencyRespone>(agencies)
            {
                Total = agencies.Count,
                Type = "Agencies"
            };
        }
        public async Task<ResponseModel<AgencyRespone>> SearchAgencies(PaginationRequest model, string value)
        {
            var agencies = await _context.Agencies.Where(a => a.Company.CompanyName.Contains(value) || a.Account.Username.Contains(value)
            || a.ManagerName.Contains(value) || a.AgencyName.Contains(value) || a.Address.Contains(value) || a.Telephone.Contains(value)).Select(a => new AgencyRespone
            {
                id = a.Id,
                agency_name = a.AgencyName,
                username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
                address = a.Address,
                company_name = _context.Companies.Where(x => x.Id.Equals(a.CompanyId)).Select(x => x.CompanyName).FirstOrDefault(),
                manager_name = a.ManagerName,
                telephone = a.Telephone,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
            }).OrderBy(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AgencyRespone>(agencies)
            {
                Total = agencies.Count,
                Type = "Agencies"
            };
        }
        public async Task<ResponseModel<AgencyRespone>> CreateAgency(AgencyRequest model)
        {
            var agency = new Agency
            {
                Id = Guid.NewGuid(),
                AgencyName = model.agency_name,
                AccountId = _context.Accounts.Where(x => x.Username.Equals(model.username)).Select(x => x.Id).FirstOrDefault(),
                Address = model.address,
                CompanyId = _context.Companies.Where(x => x.CompanyName.Equals(model.company_name)).Select(x => x.Id).FirstOrDefault(),
                ManagerName = model.manager_name,
                Telephone = model.telephone,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = null,

            };
            var list = new List<AgencyRespone>();
            var message = "blank";
            var status = 500;
            var agency_name = await _context.Agencies.Where(x => x.AgencyName.Equals(agency.AgencyName)).FirstOrDefaultAsync();
            if (agency_name != null)
            {
                status = 400;
                message = "AgencyName is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Agencies.AddAsync(agency);
                await _context.SaveChangesAsync();
                list.Add(new AgencyRespone
                {
                    id = agency.Id,
                    agency_name = agency.AgencyName,
                    username = await _context.Accounts.Where(x => x.Id.Equals(agency.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
                    address = agency.Address,
                    company_name = await _context.Companies.Where(x => x.Id.Equals(agency.CompanyId)).Select(x => x.CompanyName).FirstOrDefaultAsync(),
                    manager_name = agency.ManagerName,
                    telephone = agency.Telephone,
                    is_delete = agency.IsDelete,
                    create_date = agency.CreateDate,
                    update_date = agency.UpdateDate,
                });
            }
            return new ResponseModel<AgencyRespone>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Agency"
            };
        }
        public async Task<ResponseModel<AgencyRespone>> UpdateAgency(Guid id, AgencyRequest model)
        {
            var agency = await _context.Agencies.Where(x => x.Id.Equals(id)).Select(x => new Agency
            {
                Id = id,
                AgencyName = model.agency_name,
                AccountId = _context.Accounts.Where(x => x.Username.Equals(model.username)).Select(x => x.Id).FirstOrDefault(),
                Address = model.address,
                CompanyId = _context.Companies.Where(x => x.CompanyName.Equals(model.company_name)).Select(x => x.Id).FirstOrDefault(),
                ManagerName = model.manager_name,
                Telephone = model.telephone,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
            }).FirstOrDefaultAsync();
            _context.Agencies.Update(agency);
            await _context.SaveChangesAsync();
            var list = new List<AgencyRespone>();
            list.Add(new AgencyRespone
            {
                id = agency.Id,
                agency_name = agency.AgencyName,
                username = await _context.Accounts.Where(x => x.Id.Equals(agency.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
                address = agency.Address,
                company_name = await _context.Companies.Where(x => x.Id.Equals(agency.CompanyId)).Select(x => x.CompanyName).FirstOrDefaultAsync(),
                manager_name = agency.ManagerName,
                telephone = agency.Telephone,
                is_delete = agency.IsDelete,
                create_date = agency.CreateDate,
                update_date = agency.UpdateDate,
            });
            return new ResponseModel<AgencyRespone>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Agency"
            };
        }

        public async Task<ResponseModel<AgencyRespone>> DisableAgency(Guid id, AgencyDisableRequest model)
        {
            var agency = await _context.Agencies.Where(x => x.Id.Equals(id)).Select(x => new Agency
            {
                Id = id,
                AgencyName = x.AgencyName,
                AccountId = _context.Accounts.Where(x => x.Username.Equals(x.Username)).Select(x => x.Id).FirstOrDefault(),
                Address = x.Address,
                CompanyId = _context.Companies.Where(x => x.CompanyName.Equals(x.CompanyName)).Select(x => x.Id).FirstOrDefault(),
                ManagerName = x.ManagerName,
                Telephone = x.Telephone,
                IsDelete = model.is_delete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
            }).FirstOrDefaultAsync();
            _context.Agencies.Update(agency);
            await _context.SaveChangesAsync();
            var list = new List<AgencyRespone>();
            list.Add(new AgencyRespone
            {
                id = agency.Id,
                agency_name = agency.AgencyName,
                username = await _context.Accounts.Where(x => x.Id.Equals(agency.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
                address = agency.Address,
                company_name = await _context.Companies.Where(x => x.Id.Equals(agency.CompanyId)).Select(x => x.CompanyName).FirstOrDefaultAsync(),
                manager_name = agency.ManagerName,
                telephone = agency.Telephone,
                is_delete = agency.IsDelete,
                create_date = agency.CreateDate,
                update_date = agency.UpdateDate,
            });
            return new ResponseModel<AgencyRespone>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Agency"
            };
        }

    }
}
