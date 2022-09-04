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
    public partial interface IServiceRepository :IBaseRepository<Service>
    {
    }
    public partial class ServiceRepository :BaseRepository<Service>, IServiceRepository
    {
         public ServiceRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

