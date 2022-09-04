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
    public partial interface IDepartmentRepository :IBaseRepository<Department>
    {
    }
    public partial class DepartmentRepository :BaseRepository<Department>, IDepartmentRepository
    {
         public DepartmentRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

