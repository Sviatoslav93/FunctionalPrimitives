using WebApp.Services.Abstractions;

namespace WebApp.Services;

public class CurrentUserService : ICurrentUserService
{
    private const long _userId = 999;

    public long UserId => _userId;
}
