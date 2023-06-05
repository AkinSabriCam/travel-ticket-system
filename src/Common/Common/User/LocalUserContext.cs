namespace Common.User;

public static class LocalUserContext
{
    private static AsyncLocal<LocalUser> _localUser = new();

    public static IUser GetUser()
    {
        return _localUser.Value;
    }
    
    public static Task SetUser(LocalUser user)
    {
        _localUser = new AsyncLocal<LocalUser>() { Value = user };
        return Task.CompletedTask;
    }
}