
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
    public partial interface ICustomerRepository :IBaseRepository<Customer>
    {
    }
    public partial class CustomerRepository :BaseRepository<Customer>, ICustomerRepository
    {
         public CustomerRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

