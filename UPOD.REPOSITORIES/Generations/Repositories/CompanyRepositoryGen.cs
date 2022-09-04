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
    public partial interface ICompanyRepository :IBaseRepository<Company>
    {
    }
    public partial class CompanyRepository :BaseRepository<Company>, ICompanyRepository
    {
         public CompanyRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

