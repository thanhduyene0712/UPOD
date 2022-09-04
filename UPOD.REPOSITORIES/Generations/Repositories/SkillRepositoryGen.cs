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
    public partial interface ISkillRepository :IBaseRepository<Skill>
    {
    }
    public partial class SkillRepository :BaseRepository<Skill>, ISkillRepository
    {
         public SkillRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

