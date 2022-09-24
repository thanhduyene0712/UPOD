
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
    public partial interface ITechnicianSv:IBaseService<Technician>
    {
    }
    public partial class TechnicianSv:BaseService<Technician>,ITechnicianSv
    {
        public TechnicianSv(IUnitOfWork unitOfWork,ITechnicianRepository repository):base(unitOfWork,repository){}
    }
}
