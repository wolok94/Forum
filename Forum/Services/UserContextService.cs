using System.Security.Claims;

namespace Forum.Services;



public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor contextHttp;

    public UserContextService(IHttpContextAccessor contextHttp)
    {
        this.contextHttp = contextHttp;
    }

    public ClaimsPrincipal User => contextHttp.HttpContext?.User;
    public int? GetId => User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
}
