
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
    public partial interface ITicketRepository :IBaseRepository<Ticket>
    {
    }
    public partial class TicketRepository :BaseRepository<Ticket>, ITicketRepository
    {
         public TicketRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

