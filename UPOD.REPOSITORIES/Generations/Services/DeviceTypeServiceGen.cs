
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
    public partial interface IDeviceTypeSv:IBaseService<DeviceType>
    {
    }
    public partial class DeviceTypeSv:BaseService<DeviceType>,IDeviceTypeSv
    {
        public DeviceTypeSv(IUnitOfWork unitOfWork,IDeviceTypeRepository repository):base(unitOfWork,repository){}
    }
}
