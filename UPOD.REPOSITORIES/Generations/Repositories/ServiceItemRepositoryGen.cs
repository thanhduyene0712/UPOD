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
    public partial interface IServiceItemRepository :IBaseRepository<ServiceItem>
    {
    }
    public partial class ServiceItemRepository :BaseRepository<ServiceItem>, IServiceItemRepository
    {
         public ServiceItemRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

