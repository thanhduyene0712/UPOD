using Microsoft.Extensions.DependencyInjection;
//using UPOD.REPOSITORIES.Commons;
using UPOD.SERVICES.Services;

namespace UPOP.SERVICES.App_Start
{
    public static class DependencyInjectionResolver
    {
        public static void ConfigureDI(this IServiceCollection services)
        {
            //services.InitializerDI();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICustomerService, CustomerServices>();
            services.AddScoped<IServiceService, ServiceServices>();
            services.AddScoped<IContractServiceService, ContractServiceService>();
            services.AddScoped<IRequestService, RequestServices>();
            services.AddScoped<IDeviceService, DeviceServices>();
            services.AddScoped<IDeviceTypeService, DeviceTypeServices>();
            services.AddScoped<IAgencyService, AgencyServices>();
            services.AddScoped<IAreaService, AreaServices>();
            services.AddScoped<IGuidelineService, GuidelineServices>();
            services.AddScoped<ITechnicianService, TechnicianServices>();
            services.AddScoped<IUserAccessor, UserAccessorServices>();
            services.AddScoped<IAdminService, AdminServices>();
            services.AddScoped<IMaintenanceScheduleService, MaintenanceScheduleServices>();

        }
    }
}
