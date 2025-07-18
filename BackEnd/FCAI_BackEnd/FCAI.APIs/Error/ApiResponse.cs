using System.Text.Json;

namespace FCAI.APIs.Error
{
    public class ApiResponce
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponce(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Request, You Are Made",
                401 => "Authorized, You Are Not",
                404 => "Resource Was Not Found",
                500 => "Server Error :(",
                _ => null
            };
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        }
    }
}
