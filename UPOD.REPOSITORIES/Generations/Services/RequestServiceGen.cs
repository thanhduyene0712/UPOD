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
    public partial interface IRequestSv:IBaseService<Request>
    {
    }
    public partial class RequestSv:BaseService<Request>,IRequestSv
    {
        public RequestSv(IUnitOfWork unitOfWork,IRequestRepository repository):base(unitOfWork,repository){}
    }
}
