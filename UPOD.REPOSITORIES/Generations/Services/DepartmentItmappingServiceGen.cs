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
    public partial interface IDepartmentItmappingSv:IBaseService<DepartmentItmapping>
    {
    }
    public partial class DepartmentItmappingSv:BaseService<DepartmentItmapping>,IDepartmentItmappingSv
    {
        public DepartmentItmappingSv(IUnitOfWork unitOfWork,IDepartmentItmappingRepository repository):base(unitOfWork,repository){}
    }
}
