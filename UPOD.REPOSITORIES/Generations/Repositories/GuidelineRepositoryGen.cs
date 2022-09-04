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
    public partial interface IGuidelineRepository :IBaseRepository<Guideline>
    {
    }
    public partial class GuidelineRepository :BaseRepository<Guideline>, IGuidelineRepository
    {
         public GuidelineRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

