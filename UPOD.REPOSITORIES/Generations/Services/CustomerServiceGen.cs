
/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////
namespace UPOD.REPOSITORIES.Services
{
    using Reso.Core.BaseConnect;
    using UPOD.REPOSITORIES.Models;
    using UPOD.REPOSITORIES.Repositories;
    public partial interface ICustomerSv:IBaseService<Customer>
    {
    }
    public partial class CustomerSv:BaseService<Customer>,ICustomerSv
    {
        public CustomerSv(IUnitOfWork unitOfWork,ICustomerRepository repository):base(unitOfWork,repository){}
    }
}
