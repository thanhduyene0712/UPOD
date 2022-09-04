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
    public partial interface IDeviceRepository :IBaseRepository<Device>
    {
    }
    public partial class DeviceRepository :BaseRepository<Device>, IDeviceRepository
    {
         public DeviceRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

