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
    public interface ICompanyService
    {
        Task<ResponseModel<CompanyRespone>> GetAll(PaginationRequest model);
        //Task<ResponseModel<CompanyRespone>> SearchAgencies(PaginationRequest model, String value);
        //Task<ResponseModel<CompanyRespone>> UpdateCompany(Guid id, CompanyRequest model);
        Task<ResponseModel<CompanyRespone>> CreateCompany(CompanyRequest model);
        //Task<ResponseModel<CompanyRespone>> DisableCompany(Guid id, CompanyDisableRequest model);


    }

    public class CompanyService : ICompanyService
    {
        private readonly Database_UPODContext _context;
        public CompanyService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<CompanyRespone>> GetAll(PaginationRequest model)
        {
            var companies = await _context.Companies.Select(a => new CompanyRespone
            {
                Id = a.Id,
                CompanyName = a.CompanyName,
                Description = a.Description,
                PercentForTechnicanExp = a.PercentForTechnicanExp,
                PercentForTechnicanRate = a.PercentForTechnicanRate,
                PercentForTechnicanFamiliarWithAgency = a.PercentForTechnicanFamiliarWithAgency,
                IsDelete = a.IsDelete,
                CreateDate = a.CreateDate,
                UpdateDate = a.UpdateDate,


            }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<CompanyRespone>(companies)
            {
                Total = companies.Count,
                Type = "Companies"
            };
        }
        //public async Task<ResponseModel<CompanyRespone>> SearchCom(PaginationRequest model, string value)
        //{
        //    var agencies = await _context.Agencies.Where(a => a.Company.CompanyName.Contains(value) || a.Account.Username.Contains(value)
        //    || a.ManagerName.Contains(value) || a.CompanyName.Contains(value) || a.Address.Contains(value) || a.Telephone.Contains(value)).Select(a => new CompanyRespone
        //    {
        //        Id = a.Id,
        //        CompanyName = a.CompanyName,
        //        Username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
        //        Address = a.Address,
        //        CompanyName = _context.Companies.Where(x => x.Id.Equals(a.CompanyId)).Select(x => x.CompanyName).FirstOrDefault(),
        //        ManagerName = a.ManagerName,
        //        Telephone = a.Telephone,
        //        IsDelete = a.IsDelete,
        //        CreateDate = a.CreateDate,
        //        UpdateDate = a.UpdateDate,
        //    }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
        //    return new ResponseModel<CompanyRespone>(agencies)
        //    {
        //        Total = agencies.Count,
        //        Type = "Agencies"
        //    };
        //}
        public async Task<ResponseModel<CompanyRespone>> CreateCompany(CompanyRequest model)
        {
            var company = new Company
            {
                Id = Guid.NewGuid(),
                CompanyName = model.CompanyName,
                Description = model.Description,
                PercentForTechnicanExp = model.PercentForTechnicanExp,
                PercentForTechnicanRate = model.PercentForTechnicanRate,
                PercentForTechnicanFamiliarWithAgency = model.PercentForTechnicanFamiliarWithAgency,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = null,

            };
            var list = new List<CompanyRespone>();
            var message = "blank";
            var status = 500;
            var company_name = await _context.Companies.Where(x => x.CompanyName.Equals(company.CompanyName)).FirstOrDefaultAsync();
            if (company_name != null)
            {
                status = 400;
                message = "CompanyName is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Companies.AddAsync(company);
                await _context.SaveChangesAsync();
                list.Add(new CompanyRespone
                {
                    Id = company.Id,
                    CompanyName = company.CompanyName,
                    Description = company.Description,
                    PercentForTechnicanExp = company.PercentForTechnicanExp,
                    PercentForTechnicanRate = company.PercentForTechnicanRate,
                    PercentForTechnicanFamiliarWithAgency = company.PercentForTechnicanFamiliarWithAgency,
                    IsDelete = company.IsDelete,
                    CreateDate = company.CreateDate,
                    UpdateDate = company.UpdateDate,
                });
            }
            return new ResponseModel<CompanyRespone>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Company"
            };
        }
        //public async Task<ResponseModel<CompanyRespone>> UpdateCompany(Guid id, CompanyRequest model)
        //{
        //    var Company = await _context.Agencies.Where(x => x.Id.Equals(id)).Select(x => new Company
        //    {
        //        Id = id,
        //        CompanyName = model.CompanyName,
        //        AccountId = _context.Accounts.Where(x => x.Username.Equals(model.Username)).Select(x => x.Id).FirstOrDefault(),
        //        Address = model.Address,
        //        CompanyId = _context.Companies.Where(x => x.CompanyName.Equals(model.CompanyName)).Select(x => x.Id).FirstOrDefault(),
        //        ManagerName = model.ManagerName,
        //        Telephone = model.Telephone,
        //        IsDelete = x.IsDelete,
        //        CreateDate = x.CreateDate,
        //        UpdateDate = DateTime.Now,
        //    }).FirstOrDefaultAsync();
        //    _context.Agencies.Update(Company);
        //    await _context.SaveChangesAsync();
        //    var list = new List<CompanyRespone>();
        //    list.Add(new CompanyRespone
        //    {
        //        Id = Company.Id,
        //        CompanyName = Company.CompanyName,
        //        Username = await _context.Accounts.Where(x => x.Id.Equals(Company.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
        //        Address = Company.Address,
        //        CompanyName = await _context.Companies.Where(x => x.Id.Equals(Company.CompanyId)).Select(x => x.CompanyName).FirstOrDefaultAsync(),
        //        ManagerName = Company.ManagerName,
        //        Telephone = Company.Telephone,
        //        IsDelete = Company.IsDelete,
        //        CreateDate = Company.CreateDate,
        //        UpdateDate = Company.UpdateDate,
        //    });
        //    return new ResponseModel<CompanyRespone>(list)
        //    {
        //        Status = 201,
        //        Total = list.Count,
        //        Type = "Company"
        //    };
        //}

        //public async Task<ResponseModel<CompanyRespone>> DisableCompany(Guid id, CompanyDisableRequest model)
        //{
        //    var Company = await _context.Agencies.Where(x => x.Id.Equals(id)).Select(x => new Company
        //    {
        //        Id = id,
        //        CompanyName = x.CompanyName,
        //        AccountId = _context.Accounts.Where(x => x.Username.Equals(x.Username)).Select(x => x.Id).FirstOrDefault(),
        //        Address = x.Address,
        //        CompanyId = _context.Companies.Where(x => x.CompanyName.Equals(x.CompanyName)).Select(x => x.Id).FirstOrDefault(),
        //        ManagerName = x.ManagerName,
        //        Telephone = x.Telephone,
        //        IsDelete = model.IsDelete,
        //        CreateDate = x.CreateDate,
        //        UpdateDate = DateTime.Now,
        //    }).FirstOrDefaultAsync();
        //    _context.Agencies.Update(Company);
        //    await _context.SaveChangesAsync();
        //    var list = new List<CompanyRespone>();
        //    list.Add(new CompanyRespone
        //    {
        //        Id = Company.Id,
        //        CompanyName = Company.CompanyName,
        //        Username = await _context.Accounts.Where(x => x.Id.Equals(Company.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
        //        Address = Company.Address,
        //        CompanyName = await _context.Companies.Where(x => x.Id.Equals(Company.CompanyId)).Select(x => x.CompanyName).FirstOrDefaultAsync(),
        //        ManagerName = Company.ManagerName,
        //        Telephone = Company.Telephone,
        //        IsDelete = Company.IsDelete,
        //        CreateDate = Company.CreateDate,
        //        UpdateDate = Company.UpdateDate,
        //    });
        //    return new ResponseModel<CompanyRespone>(list)
        //    {
        //        Status = 201,
        //        Total = list.Count,
        //        Type = "Company"
        //    };
        //}

    }
}
