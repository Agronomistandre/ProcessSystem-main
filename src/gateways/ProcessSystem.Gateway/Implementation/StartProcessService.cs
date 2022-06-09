using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ProcessSystem.DB;

namespace ProcessSystem.Implementation
{
    public interface IStartProcessService
    {
        public Task<string> ProcessStartAsync(string token);
    }
    public class StartProcessService : IStartProcessService
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly HttpClient _httpClient;

        public StartProcessService(IRegisterRepository registerRepository, HttpClient httpClient)
        {

            _registerRepository = registerRepository;
            _httpClient = httpClient;
        }

        public async Task<string> ProcessStartAsync(string token)
        {
            await Task.Delay(1000);
            var result = await _registerRepository.FindByTokenAsync(token);
            var jsonString = JsonSerializer.Serialize(result.Name);
            var content = new StringContent(jsonString);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var responce = await _httpClient.PostAsync(result.Url, content);
            if (responce.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return "Ok!";
            }
            return "Not ok?!";
        }
    }
}
