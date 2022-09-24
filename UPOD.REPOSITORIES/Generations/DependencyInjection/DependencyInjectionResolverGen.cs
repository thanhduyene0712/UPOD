

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
        

            services.AddScoped<IAreaSv, AreaSv>();
            services.AddScoped<IAreaRepository, AreaRepository>();
        

            services.AddScoped<IContractSv, ContractSv>();
            services.AddScoped<IContractRepository, ContractRepository>();
        

            services.AddScoped<IContractServiceSv, ContractServiceSv>();
            services.AddScoped<IContractServiceRepository, ContractServiceRepository>();
        

            services.AddScoped<ICustomerSv, CustomerSv>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        

            services.AddScoped<IDeviceSv, DeviceSv>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
        

            services.AddScoped<IDeviceTypeSv, DeviceTypeSv>();
            services.AddScoped<IDeviceTypeRepository, DeviceTypeRepository>();
        

            services.AddScoped<IGuidelineSv, GuidelineSv>();
            services.AddScoped<IGuidelineRepository, GuidelineRepository>();
        

            services.AddScoped<IRequestSv, RequestSv>();
            services.AddScoped<IRequestRepository, RequestRepository>();
        

            services.AddScoped<IRequestHistorySv, RequestHistorySv>();
            services.AddScoped<IRequestHistoryRepository, RequestHistoryRepository>();
        

            services.AddScoped<IRoleSv, RoleSv>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        

            services.AddScoped<IServiceSv, ServiceSv>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
        

            services.AddScoped<ISkillSv, SkillSv>();
            services.AddScoped<ISkillRepository, SkillRepository>();
        

            services.AddScoped<ITechnicianSv, TechnicianSv>();
            services.AddScoped<ITechnicianRepository, TechnicianRepository>();
        

            services.AddScoped<ITicketSv, TicketSv>();
            services.AddScoped<ITicketRepository, TicketRepository>();
        
}
    }
}
