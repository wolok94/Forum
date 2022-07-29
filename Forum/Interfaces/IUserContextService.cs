using System.Security.Claims;

namespace Forum;

public interface IUserContextService
{
    int? GetId { get; }
    ClaimsPrincipal User { get; }
}
