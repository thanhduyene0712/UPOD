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
    public partial interface IRequestRepository :IBaseRepository<Request>
    {
    }
    public partial class RequestRepository :BaseRepository<Request>, IRequestRepository
    {
         public RequestRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

