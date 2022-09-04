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
    public partial interface IRequestCategorySv:IBaseService<RequestCategory>
    {
    }
    public partial class RequestCategorySv:BaseService<RequestCategory>,IRequestCategorySv
    {
        public RequestCategorySv(IUnitOfWork unitOfWork,IRequestCategoryRepository repository):base(unitOfWork,repository){}
    }
}
