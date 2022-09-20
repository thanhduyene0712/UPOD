
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
    public partial interface IAgencyDeviceSv:IBaseService<AgencyDevice>
    {
    }
    public partial class AgencyDeviceSv:BaseService<AgencyDevice>,IAgencyDeviceSv
    {
        public AgencyDeviceSv(IUnitOfWork unitOfWork,IAgencyDeviceRepository repository):base(unitOfWork,repository){}
    }
}
