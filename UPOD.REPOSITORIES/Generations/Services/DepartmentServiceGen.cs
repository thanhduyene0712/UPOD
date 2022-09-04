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
    public partial interface IDepartmentSv:IBaseService<Department>
    {
    }
    public partial class DepartmentSv:BaseService<Department>,IDepartmentSv
    {
        public DepartmentSv(IUnitOfWork unitOfWork,IDepartmentRepository repository):base(unitOfWork,repository){}
    }
}
