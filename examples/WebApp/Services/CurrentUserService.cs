using WebApp.Services.Abstractions;

namespace WebApp.Services;

public class CurrentUserService : ICurrentUserService
{
    private const long _userId = 1;

    public long UserId => _userId;
}
