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
    public partial interface IRequestCategoryRepository :IBaseRepository<RequestCategory>
    {
    }
    public partial class RequestCategoryRepository :BaseRepository<RequestCategory>, IRequestCategoryRepository
    {
         public RequestCategoryRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

