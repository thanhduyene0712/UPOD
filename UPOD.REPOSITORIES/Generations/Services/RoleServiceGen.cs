
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
    public partial interface IRoleSv:IBaseService<Role>
    {
    }
    public partial class RoleSv:BaseService<Role>,IRoleSv
    {
        public RoleSv(IUnitOfWork unitOfWork,IRoleRepository repository):base(unitOfWork,repository){}
    }
}
