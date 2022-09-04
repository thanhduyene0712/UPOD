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
    public partial interface IRoleRepository :IBaseRepository<Role>
    {
    }
    public partial class RoleRepository :BaseRepository<Role>, IRoleRepository
    {
         public RoleRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

