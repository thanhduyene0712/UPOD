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
    public partial interface IRequestTaskRepository :IBaseRepository<RequestTask>
    {
    }
    public partial class RequestTaskRepository :BaseRepository<RequestTask>, IRequestTaskRepository
    {
         public RequestTaskRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

