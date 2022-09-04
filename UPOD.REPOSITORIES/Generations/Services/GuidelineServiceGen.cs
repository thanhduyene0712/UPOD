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
    public partial interface IGuidelineSv:IBaseService<Guideline>
    {
    }
    public partial class GuidelineSv:BaseService<Guideline>,IGuidelineSv
    {
        public GuidelineSv(IUnitOfWork unitOfWork,IGuidelineRepository repository):base(unitOfWork,repository){}
    }
}
