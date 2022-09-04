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
    public partial interface IRequestHistorySv:IBaseService<RequestHistory>
    {
    }
    public partial class RequestHistorySv:BaseService<RequestHistory>,IRequestHistorySv
    {
        public RequestHistorySv(IUnitOfWork unitOfWork,IRequestHistoryRepository repository):base(unitOfWork,repository){}
    }
}
