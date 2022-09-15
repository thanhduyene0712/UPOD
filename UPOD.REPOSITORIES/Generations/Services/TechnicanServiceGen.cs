
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
    public partial interface ITechnicanSv:IBaseService<Technican>
    {
    }
    public partial class TechnicanSv:BaseService<Technican>,ITechnicanSv
    {
        public TechnicanSv(IUnitOfWork unitOfWork,ITechnicanRepository repository):base(unitOfWork,repository){}
    }
}
