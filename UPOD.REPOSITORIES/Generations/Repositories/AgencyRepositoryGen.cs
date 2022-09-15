
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
    public partial interface IAgencyRepository :IBaseRepository<Agency>
    {
    }
    public partial class AgencyRepository :BaseRepository<Agency>, IAgencyRepository
    {
         public AgencyRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

