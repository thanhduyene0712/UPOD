using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reso.Core.Utilities;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Services;
using ICompanyService = UPOD.SERVICES.Services.ICompanyService;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public partial class CompaniesController : ControllerBase
    {

        private readonly ICompanyService _company_sv;
        public CompaniesController(ICompanyService company_sv)
        {
            _company_sv = company_sv;
        }

        [HttpGet]
        [Route("get_all_companies")]
        public async Task<ResponseModel<CompanyResponse>> GetAllCompanies([FromQuery] PaginationRequest model)
        {
            return await _company_sv.GetAll(model);
        }
        [HttpGet]
        [Route("get_company_details_by_id")]
        public async Task<ResponseModel<CompanyResponse>> GetCompanyDetails([FromQuery] PaginationRequest model, Guid id)
        {
            return await _company_sv.GetCompanyDetails(model, id);
        }

        [HttpPost]
        [Route("create_companies")]
        public async Task<ResponseModel<CompanyResponse>> CreateCompany(CompanyRequest model)
        {
            return await _company_sv.CreateCompany(model);
        }
        [HttpPut]
        [Route("update_comnpany_by_id")]
        public async Task<ResponseModel<CompanyResponse>> UpdateCompany(Guid id, CompanyRequest model)
        {
            return await _company_sv.UpdateCompany(id, model);
        }

        [HttpPut]
        [Route("disable_company_by_id")]
        public async Task<ResponseModel<CompanyResponse>> DisableCompany(Guid id)
        {
            return await _company_sv.DisableCompany(id);
        }


    }
}
