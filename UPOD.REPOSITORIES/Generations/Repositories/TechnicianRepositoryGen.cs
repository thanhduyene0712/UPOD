
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
    public partial interface ITechnicianRepository :IBaseRepository<Technician>
    {
    }
    public partial class TechnicianRepository :BaseRepository<Technician>, ITechnicianRepository
    {
         public TechnicianRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

