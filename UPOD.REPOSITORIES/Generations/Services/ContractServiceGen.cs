
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
    public partial interface IContractSv:IBaseService<Contract>
    {
    }
    public partial class ContractSv:BaseService<Contract>,IContractSv
    {
        public ContractSv(IUnitOfWork unitOfWork,IContractRepository repository):base(unitOfWork,repository){}
    }
}
