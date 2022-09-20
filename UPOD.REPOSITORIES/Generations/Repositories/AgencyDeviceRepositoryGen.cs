
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
    public partial interface IAgencyDeviceRepository :IBaseRepository<AgencyDevice>
    {
    }
    public partial class AgencyDeviceRepository :BaseRepository<AgencyDevice>, IAgencyDeviceRepository
    {
         public AgencyDeviceRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

