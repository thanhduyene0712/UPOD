using Microsoft.Extensions.DependencyInjection;
using UPOD.REPOSITORIES.Commons;
using UPOD.SERVICES.Services;

namespace UPOP.SERVICES.App_Start
{
    public static class DependencyInjectionResolver
    {
        public static void ConfigureDI(this IServiceCollection services)
        {
            services.InitializerDI();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IContractServiceService, ContractServiceService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IDeviceService, DeviceService>();


        }
    }
}
