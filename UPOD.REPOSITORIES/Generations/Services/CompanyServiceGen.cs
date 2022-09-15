
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
    public partial interface ICompanySv:IBaseService<Company>
    {
    }
    public partial class CompanySv:BaseService<Company>,ICompanySv
    {
        public CompanySv(IUnitOfWork unitOfWork,ICompanyRepository repository):base(unitOfWork,repository){}
    }
}
