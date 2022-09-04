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
    public partial interface ITicketSv:IBaseService<Ticket>
    {
    }
    public partial class TicketSv:BaseService<Ticket>,ITicketSv
    {
        public TicketSv(IUnitOfWork unitOfWork,ITicketRepository repository):base(unitOfWork,repository){}
    }
}
