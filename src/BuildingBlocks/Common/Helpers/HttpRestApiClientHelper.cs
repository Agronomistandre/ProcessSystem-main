using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.Helpers
{
    public static class HttpRestApiClientHelper
    {

        public static async Task<T> GetAsync<T>(string url, HttpClient client, ILogger? logger)
        {
            using var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                logger?.LogDebug("GetAsync успешно выполнен");
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            logger?.LogDebug($"GetAsync неуспешный статус {response.StatusCode}");
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(),
                new ArgumentException("GetAsync неуспешный статус", $"{response.StatusCode} : {response.Headers} : {response.RequestMessage}"));
        }

        public static async Task<T> GetAsync<T>(string url, StringContent content, HttpClient client, ILogger? logger)
        {
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            logger?.LogDebug($"GetAsync<T> контент запроса {await content.ReadAsStringAsync()}");

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Content = content
            };
            using var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                logger?.LogDebug("GetAsync успешно выполнен");
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            logger?.LogDebug($"GetAsync неуспешный статус {response.StatusCode}");
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(),
                new ArgumentException("GetAsync неуспешный статус", $"{response.StatusCode} : {response.Headers} : {response.RequestMessage}"));
        }

        public static async Task<HttpResponseMessage> GetAsync(string url, HttpClient client, ILogger? logger)
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                logger?.LogDebug("GetAsync успешно выполнен");
                return response;
            }
            logger?.LogDebug($"GetAsync неуспешный статус {response.StatusCode}");
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(),
                new ArgumentException("GetAsync неуспешный статус", $"{response.StatusCode} : {response.Headers} : {response.RequestMessage}"));
        }

        public static async Task<HttpResponseMessage> PostAsync(string url, HttpClient client, StringContent content, ILogger? logger)
        {
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            logger?.LogDebug($"PostAsync контент запроса {await content.ReadAsStringAsync()}");

            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                logger?.LogDebug("PostAsync успешно выполнен");
                return response;
            }

            logger?.LogDebug($"PostAsync неуспешный статус {response.StatusCode}");
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(),
                new ArgumentException("PostAsync неуспешный статус", $"{response.StatusCode} : {response.Headers} : {response.RequestMessage}"));
        }

        public static async Task<T> PostAsync<T>(string url, HttpClient client, StringContent content, ILogger? logger)
        {
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            logger?.LogDebug($"PostAsync<T> контент запроса {await content.ReadAsStringAsync()}");

            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                logger?.LogDebug("PostAsync<T> успешно выполнен");
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }

            logger?.LogDebug($"PostAsync<T> неуспешный статус {response.StatusCode}");
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(),
                new ArgumentException("PostAsync<T> неуспешный статус", $"{response.StatusCode} : {response.Headers} : {response.RequestMessage}"));
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string url, HttpClient client, StringContent content, ILogger? logger)
        {
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            logger?.LogDebug($"DeleteAsync контент запроса {await content.ReadAsStringAsync()}");

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url),
                Content = content
            };

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                logger?.LogDebug("DeleteAsync успешно выполнен");
                return response;
            }

            logger?.LogDebug($"DeleteAsync неуспешный статус {response.StatusCode}");
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(),
                new ArgumentException("DeleteAsync неуспешный статус", $"{response.StatusCode} : {response.Headers} : {response.RequestMessage}"));
        }

        public static async Task<HttpResponseMessage> PutAsync(string url, HttpClient client, StringContent content, ILogger? logger)
        {
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            logger?.LogDebug($"PutAsync контент запроса {await content.ReadAsStringAsync()}");

            var response = await client.PutAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                logger?.LogDebug("PutAsync успешно выполнен");
                return response;
            }

            logger?.LogDebug($"PutAsync неуспешный статус {response.StatusCode}");
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(),
                new ArgumentException("PutAsync неуспешный статус", $"{response.StatusCode} : {response.Headers} : {response.RequestMessage}"));
        }
    }
}
