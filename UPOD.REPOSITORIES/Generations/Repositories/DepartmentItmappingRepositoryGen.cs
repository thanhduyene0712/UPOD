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
    public partial interface IDepartmentItmappingRepository :IBaseRepository<DepartmentItmapping>
    {
    }
    public partial class DepartmentItmappingRepository :BaseRepository<DepartmentItmapping>, IDepartmentItmappingRepository
    {
         public DepartmentItmappingRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

