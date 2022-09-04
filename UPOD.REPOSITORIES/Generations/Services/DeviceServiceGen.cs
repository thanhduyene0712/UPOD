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
    public partial interface IDeviceSv:IBaseService<Device>
    {
    }
    public partial class DeviceSv:BaseService<Device>,IDeviceSv
    {
        public DeviceSv(IUnitOfWork unitOfWork,IDeviceRepository repository):base(unitOfWork,repository){}
    }
}
