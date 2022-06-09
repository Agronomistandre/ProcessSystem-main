using Newtonsoft.Json;

namespace ProcessSystem.Contracts
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BaseResponse<T>
    {
        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public T Data { get; set; }
    }
}
