
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
    public partial interface IAreaSv:IBaseService<Area>
    {
    }
    public partial class AreaSv:BaseService<Area>,IAreaSv
    {
        public AreaSv(IUnitOfWork unitOfWork,IAreaRepository repository):base(unitOfWork,repository){}
    }
}
