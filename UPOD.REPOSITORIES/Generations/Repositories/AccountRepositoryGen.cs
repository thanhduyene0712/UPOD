

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
    public partial interface IAccountRepository :IBaseRepository<Account>
    {
    }
    public partial class AccountRepository :BaseRepository<Account>, IAccountRepository
    {
         public AccountRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

