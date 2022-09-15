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
    [Route("api/Companies")]
    public partial class CompaniesController : ControllerBase
    {

        private readonly ICompanyService _companySv;
        public CompaniesController(ICompanyService companySv)
        {
            _companySv = companySv;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ResponseModel<CompanyRespone>> GetAllCompanies([FromQuery] PaginationRequest model)
        {
            return await _companySv.GetAll(model);
        }

        //[HttpGet]
        //[Route("Search")]
        //public async Task<ResponseModel<CompanyRespone>> SearchAgencies([FromQuery] PaginationRequest model, String value)
        //{
        //    return await _CompanySv.SearchAgencies(model, value);
        //}
        [HttpPost]
        [Route("Create")]
        public async Task<ResponseModel<CompanyRespone>> CreateCompany([FromQuery] CompanyRequest model)
        {
            return await _companySv.CreateCompany(model);
        }
        //[HttpPut]
        //[Route("Update")]
        //public async Task<ResponseModel<CompanyRespone>> UpdateCompany(Guid id, [FromQuery] CompanyRequest model)
        //{
        //    return await _CompanySv.UpdateCompany(id, model);
        //}
        //[HttpPut]
        //[Route("Deisable")]
        //public async Task<ResponseModel<CompanyRespone>> DisableCompany(Guid id, [FromQuery] CompanyDisableRequest model)
        //{
        //    return await _CompanySv.DisableCompany(id, model);
        //}


    }
}
