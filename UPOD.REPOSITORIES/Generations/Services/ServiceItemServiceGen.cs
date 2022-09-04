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
    public partial interface IServiceItemSv:IBaseService<ServiceItem>
    {
    }
    public partial class ServiceItemSv:BaseService<ServiceItem>,IServiceItemSv
    {
        public ServiceItemSv(IUnitOfWork unitOfWork,IServiceItemRepository repository):base(unitOfWork,repository){}
    }
}
