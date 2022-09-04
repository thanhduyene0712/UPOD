using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace UPOP.SERVICES.App_Start
{
    public static class AutoMapperConfig
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
             
        }
    }
}
