
/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using UPOD.REPOSITORIES.Models;
using Microsoft.Extensions.DependencyInjection;
using UPOD.REPOSITORIES.Services;
using UPOD.REPOSITORIES.Repositories;
using Microsoft.EntityFrameworkCore;
using Reso.Core.BaseConnect;
namespace UPOD.REPOSITORIES.Commons
{
    public static class DependencyInjectionResolverGen
    {
        public static void InitializerDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbContext, Database_UPODContext>();
        
            services.AddScoped<IAccountSv, AccountSv>();
            services.AddScoped<IAccountRepository, AccountRepository>();
        
            services.AddScoped<IAgencySv, AgencySv>();
            services.AddScoped<IAgencyRepository, AgencyRepository>();
        
            services.AddScoped<ICompanySv, CompanySv>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
        
            services.AddScoped<IContractSv, ContractSv>();
            services.AddScoped<IContractRepository, ContractRepository>();
        
            services.AddScoped<IContractServiceSv, ContractServiceSv>();
            services.AddScoped<IContractServiceRepository, ContractServiceRepository>();
        
            services.AddScoped<IDepartmentSv, DepartmentSv>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        
            services.AddScoped<IDepartmentItmappingSv, DepartmentItmappingSv>();
            services.AddScoped<IDepartmentItmappingRepository, DepartmentItmappingRepository>();
        
            services.AddScoped<IDeviceSv, DeviceSv>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
        
            services.AddScoped<IDeviceTypeSv, DeviceTypeSv>();
            services.AddScoped<IDeviceTypeRepository, DeviceTypeRepository>();
        
            services.AddScoped<IGuidelineSv, GuidelineSv>();
            services.AddScoped<IGuidelineRepository, GuidelineRepository>();
        
            services.AddScoped<IRequestSv, RequestSv>();
            services.AddScoped<IRequestRepository, RequestRepository>();
        
            services.AddScoped<IRequestCategorySv, RequestCategorySv>();
            services.AddScoped<IRequestCategoryRepository, RequestCategoryRepository>();
        
            services.AddScoped<IRequestHistorySv, RequestHistorySv>();
            services.AddScoped<IRequestHistoryRepository, RequestHistoryRepository>();
        
            services.AddScoped<IRequestTaskSv, RequestTaskSv>();
            services.AddScoped<IRequestTaskRepository, RequestTaskRepository>();
        
            services.AddScoped<IRoleSv, RoleSv>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        
            services.AddScoped<IServiceSv, ServiceSv>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
        
            services.AddScoped<IServiceItemSv, ServiceItemSv>();
            services.AddScoped<IServiceItemRepository, ServiceItemRepository>();
        
            services.AddScoped<ISkillSv, SkillSv>();
            services.AddScoped<ISkillRepository, SkillRepository>();
        
            services.AddScoped<ITechnicanSv, TechnicanSv>();
            services.AddScoped<ITechnicanRepository, TechnicanRepository>();
        
            services.AddScoped<ITicketSv, TicketSv>();
            services.AddScoped<ITicketRepository, TicketRepository>();
        }
    }
}
