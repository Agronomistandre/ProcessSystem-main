namespace ProcessSystem.UI.Blazor.Server.Services
{
    public interface ITokenCacheService
    {
        string GetAccessToken(string key);
        string SetAccessToken(string key, string token);
    }
}