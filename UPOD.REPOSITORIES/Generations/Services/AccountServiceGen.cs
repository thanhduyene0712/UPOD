








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
    public partial interface IAccountSv:IBaseService<Account>
    {
    }
    public partial class AccountSv:BaseService<Account>,IAccountSv
    {
        public AccountSv(IUnitOfWork unitOfWork,IAccountRepository repository):base(unitOfWork,repository){}
    }
}
