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
    public partial interface IRequestTaskSv:IBaseService<RequestTask>
    {
    }
    public partial class RequestTaskSv:BaseService<RequestTask>,IRequestTaskSv
    {
        public RequestTaskSv(IUnitOfWork unitOfWork,IRequestTaskRepository repository):base(unitOfWork,repository){}
    }
}
