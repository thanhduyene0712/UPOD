
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
    public partial interface IRequestHistoryRepository :IBaseRepository<RequestHistory>
    {
    }
    public partial class RequestHistoryRepository :BaseRepository<RequestHistory>, IRequestHistoryRepository
    {
         public RequestHistoryRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

