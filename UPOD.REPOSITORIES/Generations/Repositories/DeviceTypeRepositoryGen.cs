
/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Reso.Core.BaseConnect;
using UPOD.REPOSITORIES.Models;
namespace UPOD.REPOSITORIES.Repositories
{
    public partial interface IDeviceTypeRepository :IBaseRepository<DeviceType>
    {
    }
    public partial class DeviceTypeRepository :BaseRepository<DeviceType>, IDeviceTypeRepository
    {
         public DeviceTypeRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

