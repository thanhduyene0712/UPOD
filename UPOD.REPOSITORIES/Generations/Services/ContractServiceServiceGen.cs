
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
    public partial interface IContractServiceSv:IBaseService<ContractService>
    {
    }
    public partial class ContractServiceSv:BaseService<ContractService>,IContractServiceSv
    {
        public ContractServiceSv(IUnitOfWork unitOfWork,IContractServiceRepository repository):base(unitOfWork,repository){}
    }
}
