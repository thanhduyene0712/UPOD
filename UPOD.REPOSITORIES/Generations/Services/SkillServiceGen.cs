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
    public partial interface ISkillSv:IBaseService<Skill>
    {
    }
    public partial class SkillSv:BaseService<Skill>,ISkillSv
    {
        public SkillSv(IUnitOfWork unitOfWork,ISkillRepository repository):base(unitOfWork,repository){}
    }
}
