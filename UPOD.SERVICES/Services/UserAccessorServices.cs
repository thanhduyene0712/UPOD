using Microsoft.AspNetCore.Http;

namespace UPOD.SERVICES.Services
{
    public interface IUserAccessor
    {
        Guid GetUserId();
        Guid GetRoleId();
        string GetCode();
    }
    public class UserAccessorServices : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessorServices(IHttpContextAccessor httpcontextAccessor)
        {
            _httpContextAccessor = httpcontextAccessor;
        }
        public Guid GetUserId()
        {
            return Guid.Parse(_httpContextAccessor.HttpContext!.User.Claims.SingleOrDefault(p => p.Type == "AccountId")!.Value);
        }
        public Guid GetRoleId()
        {
            return Guid.Parse(_httpContextAccessor.HttpContext!.User.Claims.SingleOrDefault(p => p.Type == "RoleId")!.Value);
        }
        public string GetCode()
        {
            return _httpContextAccessor.HttpContext!.User.Claims.SingleOrDefault(p => p.Type == "Code")!.Value;
        }
    }
}
