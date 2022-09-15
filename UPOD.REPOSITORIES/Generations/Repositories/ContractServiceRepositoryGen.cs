
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
    public partial interface IContractServiceRepository :IBaseRepository<ContractService>
    {
    }
    public partial class ContractServiceRepository :BaseRepository<ContractService>, IContractServiceRepository
    {
         public ContractServiceRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

