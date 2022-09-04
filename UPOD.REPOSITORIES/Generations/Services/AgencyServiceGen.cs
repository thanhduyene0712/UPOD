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
    public partial interface IAgencySv:IBaseService<Agency>
    {
    }
    public partial class AgencySv:BaseService<Agency>,IAgencySv
    {
        public AgencySv(IUnitOfWork unitOfWork,IAgencyRepository repository):base(unitOfWork,repository){}
    }
}
