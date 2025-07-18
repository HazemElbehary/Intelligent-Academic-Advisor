using Newtonsoft.Json;

namespace FCAI.Application.Abstraction.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiExceptionResponse : Exception
    {
        [JsonProperty]
        public int StatusCode { get; set; }

        [JsonProperty]
        public string? Message { get; set; }

        [JsonProperty]
        public string? Details { get; set; }

        public ApiExceptionResponse(int statusCode, string? message = null, string? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
        }
    }
}
