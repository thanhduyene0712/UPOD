
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
    public partial interface ITechnicanRepository :IBaseRepository<Technican>
    {
    }
    public partial class TechnicanRepository :BaseRepository<Technican>, ITechnicanRepository
    {
         public TechnicanRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

