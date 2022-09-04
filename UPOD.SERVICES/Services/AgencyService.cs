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
                Id = a.Id,
                AgencyName = a.AgencyName,
                Username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
                Address = a.Address,
                CompanyName = _context.Companies.Where(x => x.Id.Equals(a.CompanyId)).Select(x => x.CompanyName).FirstOrDefault(),
                ManagerName = a.ManagerName,
                Telephone = a.Telephone,
                IsDelete = a.IsDelete,
                CreateDate = a.CreateDate,
                UpdateDate = a.UpdateDate,


            }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
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
                Id = a.Id,
                AgencyName = a.AgencyName,
                Username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
                Address = a.Address,
                CompanyName = _context.Companies.Where(x => x.Id.Equals(a.CompanyId)).Select(x => x.CompanyName).FirstOrDefault(),
                ManagerName = a.ManagerName,
                Telephone = a.Telephone,
                IsDelete = a.IsDelete,
                CreateDate = a.CreateDate,
                UpdateDate = a.UpdateDate,
            }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
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
                AgencyName = model.AgencyName,
                AccountId = _context.Accounts.Where(x => x.Username.Equals(model.Username)).Select(x => x.Id).FirstOrDefault(),
                Address = model.Address,
                CompanyId = _context.Companies.Where(x => x.CompanyName.Equals(model.CompanyName)).Select(x => x.Id).FirstOrDefault(),
                ManagerName = model.ManagerName,
                Telephone = model.Telephone,
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
                    Id = agency.Id,
                    AgencyName = agency.AgencyName,
                    Username = await _context.Accounts.Where(x => x.Id.Equals(agency.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
                    Address = agency.Address,
                    CompanyName = await _context.Companies.Where(x => x.Id.Equals(agency.CompanyId)).Select(x => x.CompanyName).FirstOrDefaultAsync(),
                    ManagerName = agency.ManagerName,
                    Telephone = agency.Telephone,
                    IsDelete = agency.IsDelete,
                    CreateDate = agency.CreateDate,
                    UpdateDate = agency.UpdateDate,
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
                AgencyName = model.AgencyName,
                AccountId = _context.Accounts.Where(x => x.Username.Equals(model.Username)).Select(x => x.Id).FirstOrDefault(),
                Address = model.Address,
                CompanyId = _context.Companies.Where(x => x.CompanyName.Equals(model.CompanyName)).Select(x => x.Id).FirstOrDefault(),
                ManagerName = model.ManagerName,
                Telephone = model.Telephone,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
            }).FirstOrDefaultAsync();
            _context.Agencies.Update(agency);
            await _context.SaveChangesAsync();
            var list = new List<AgencyRespone>();
            list.Add(new AgencyRespone
            {
                Id = agency.Id,
                AgencyName = agency.AgencyName,
                Username = await _context.Accounts.Where(x => x.Id.Equals(agency.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
                Address = agency.Address,
                CompanyName = await _context.Companies.Where(x => x.Id.Equals(agency.CompanyId)).Select(x => x.CompanyName).FirstOrDefaultAsync(),
                ManagerName = agency.ManagerName,
                Telephone = agency.Telephone,
                IsDelete = agency.IsDelete,
                CreateDate = agency.CreateDate,
                UpdateDate = agency.UpdateDate,
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
                IsDelete = model.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
            }).FirstOrDefaultAsync();
            _context.Agencies.Update(agency);
            await _context.SaveChangesAsync();
            var list = new List<AgencyRespone>();
            list.Add(new AgencyRespone
            {
                Id = agency.Id,
                AgencyName = agency.AgencyName,
                Username = await _context.Accounts.Where(x => x.Id.Equals(agency.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
                Address = agency.Address,
                CompanyName = await _context.Companies.Where(x => x.Id.Equals(agency.CompanyId)).Select(x => x.CompanyName).FirstOrDefaultAsync(),
                ManagerName = agency.ManagerName,
                Telephone = agency.Telephone,
                IsDelete = agency.IsDelete,
                CreateDate = agency.CreateDate,
                UpdateDate = agency.UpdateDate,
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
