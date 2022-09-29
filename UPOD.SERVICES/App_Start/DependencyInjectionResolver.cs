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
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IContractServiceService, ContractServiceService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IDeviceTypeService, DeviceTypeService>();
            services.AddScoped<IAgencyService, AgencyService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IGuidelineService, GuidelineService>();
            services.AddScoped<ITechnicianService, TechnicianService>();
            services.AddScoped<IUserAccessor, UserAccessorService>();
        }
    }
}
