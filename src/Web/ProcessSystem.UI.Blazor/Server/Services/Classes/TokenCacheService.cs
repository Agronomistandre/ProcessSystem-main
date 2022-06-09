using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;


namespace ProcessSystem.UI.Blazor.Server.Services
{
    public class TokenCacheService : ITokenCacheService
    {
        private readonly HttpClient _httpClient;
        private static readonly Object _lock = new Object();
        private IDistributedCache _cache;
        private const int cacheExpirationInDays = 1;

        public TokenCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public string GetAccessToken(string key)
        {
            AccessTokenItem accessToken = GetFromCache(key);
            return accessToken is not null ? accessToken.AccessToken : string.Empty;
        }

        public string SetAccessToken(string key, string token)
        {
            AccessTokenItem newAccessToken = new AccessTokenItem
                {
                    AccessToken = token,
                };
            AddToCache(key, newAccessToken);

            return newAccessToken.AccessToken;
        }

        private AccessTokenItem GetFromCache(string key)
        {
            var item = _cache.GetString(key);
            if (item != null)
                return JsonSerializer.Deserialize<AccessTokenItem>(item);

            return null;
        }

        private void AddToCache(string key, AccessTokenItem accessTokenItem)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(cacheExpirationInDays));
            lock (_lock)
                _cache.SetString(key, JsonSerializer.Serialize(accessTokenItem), options);
        }
        
        private class AccessTokenItem
        {
            public string AccessToken { get; set; } = string.Empty;
        }
    }

}