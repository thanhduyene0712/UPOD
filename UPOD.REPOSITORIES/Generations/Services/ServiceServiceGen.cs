/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////
namespace UPOD.REPOSITORIES.Services
{
    using Reso.Core.BaseConnect;
    using UPOD.REPOSITORIES.Models;
    using UPOD.REPOSITORIES.Repositories;
    public partial interface IServiceSv:IBaseService<Service>
    {
    }
    public partial class ServiceSv:BaseService<Service>,IServiceSv
    {
        public ServiceSv(IUnitOfWork unitOfWork,IServiceRepository repository):base(unitOfWork,repository){}
    }
}
