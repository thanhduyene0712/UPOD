
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
    public partial interface IContractRepository :IBaseRepository<Contract>
    {
    }
    public partial class ContractRepository :BaseRepository<Contract>, IContractRepository
    {
         public ContractRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

