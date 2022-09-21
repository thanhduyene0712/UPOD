using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;

namespace UPOD.SERVICES.Services
{
    public interface ICompanyService
    {
        Task<ResponseModel<CompanyResponse>> GetAll(PaginationRequest model);
        Task<ResponseModel<CompanyResponse>> CreateCompany(CompanyRequest model);
        Task<ResponseModel<CompanyResponse>> GetCompanyDetails(PaginationRequest model, Guid id);
        Task<ResponseModel<CompanyResponse>> UpdateCompany(Guid id, CompanyRequest model);
        Task<ResponseModel<CompanyResponse>> DisableCompany(Guid id);
    }

    public class CompanyService : ICompanyService
    {
        private readonly Database_UPODContext _context;
        public CompanyService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<CompanyResponse>> GetAll(PaginationRequest model)
        {
            var companies = await _context.Companies.Where(a => a.IsDelete == false).Select(a => new CompanyResponse
            {
                id = a.Id,
                company_name = a.CompanyName,
                description = a.Description,
                percent_for_technican_exp = a.PercentForTechnicanExp,
                percent_for_technican_rate = a.PercentForTechnicanRate,
                percent_for_technican_familiar_with_agency = a.PercentForTechnicanFamiliarWithAgency,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,


            }).OrderBy(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<CompanyResponse>(companies)
            {
                Total = companies.Count,
                Type = "Companies"
            };
        }
        public async Task<ResponseModel<CompanyResponse>> GetCompanyDetails(PaginationRequest model, Guid id)
        {
            var companies = await _context.Companies.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new CompanyResponse
            {
                id = a.Id,
                company_name = a.CompanyName,
                description = a.Description,
                percent_for_technican_exp = a.PercentForTechnicanExp,
                percent_for_technican_rate = a.PercentForTechnicanRate,
                percent_for_technican_familiar_with_agency = a.PercentForTechnicanFamiliarWithAgency,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,


            }).OrderBy(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<CompanyResponse>(companies)
            {
                Total = companies.Count,
                Type = "Companies"
            };
        }

        public async Task<ResponseModel<CompanyResponse>> CreateCompany(CompanyRequest model)
        {
            var company = new Company
            {
                Id = Guid.NewGuid(),
                CompanyName = model.company_name,
                Description = model.description,
                PercentForTechnicanExp = model.percent_for_technican_exp,
                PercentForTechnicanRate = model.percent_for_technican_rate,
                PercentForTechnicanFamiliarWithAgency = model.percent_for_technican_familiar_with_agency,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,

            };
            var list = new List<CompanyResponse>();
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
                list.Add(new CompanyResponse
                {
                    id = company.Id,
                    company_name = company.CompanyName,
                    description = company.Description,
                    percent_for_technican_exp = company.PercentForTechnicanExp,
                    percent_for_technican_rate = company.PercentForTechnicanRate,
                    percent_for_technican_familiar_with_agency = company.PercentForTechnicanFamiliarWithAgency,
                    is_delete = company.IsDelete,
                    create_date = company.CreateDate,
                    update_date = company.UpdateDate,
                });
            }
            return new ResponseModel<CompanyResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Company"
            };
        }
        public async Task<ResponseModel<CompanyResponse>> DisableCompany(Guid id)
        {
            var company = await _context.Companies.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            company.IsDelete = true;
            company.UpdateDate = DateTime.Now;
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
            var list = new List<CompanyResponse>();
            list.Add(new CompanyResponse
            {
                is_delete = company.IsDelete,
            });
            return new ResponseModel<CompanyResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Company"
            };
        }
        public async Task<ResponseModel<CompanyResponse>> UpdateCompany(Guid id, CompanyRequest model)
        {
            var company = await _context.Companies.Where(a => a.Id.Equals(id)).Select(x => new Company
            {
                Id = id,
                CompanyName = model.company_name,
                Description = model.description,
                PercentForTechnicanExp = model.percent_for_technican_exp,
                PercentForTechnicanRate = model.percent_for_technican_rate,
                PercentForTechnicanFamiliarWithAgency = model.percent_for_technican_familiar_with_agency,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
            }).FirstOrDefaultAsync();
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
            var list = new List<CompanyResponse>();
            list.Add(new CompanyResponse
            {
                id = company.Id,
                company_name = company.CompanyName,
                description = company.Description,
                percent_for_technican_exp = company.PercentForTechnicanExp,
                percent_for_technican_rate = company.PercentForTechnicanRate,
                percent_for_technican_familiar_with_agency = company.PercentForTechnicanFamiliarWithAgency,
                is_delete = company.IsDelete,
                create_date = company.CreateDate,
                update_date = company.UpdateDate,
            });
            return new ResponseModel<CompanyResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Company"
            };
        }


    }
}
