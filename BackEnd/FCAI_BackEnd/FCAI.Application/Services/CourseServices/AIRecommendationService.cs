using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.DTOs.AI_Response;
using FCAI.Application.Abstraction.Exceptions;
using FCAI.Application.Abstraction.IServices;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace FCAI.Application.Services.CourseServices
{
    public class AIRecommendationService : IAIRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _endpointUrl;

        public AIRecommendationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _endpointUrl = configuration["AIRecommendation:Endpoint"];
            if (string.IsNullOrWhiteSpace(_endpointUrl))
                throw new InvalidOperationException("AIRecommendation:Endpoint is not configured.");
        }

        public async Task<AiResponseDto> GetRecommendationAsync(AiRequestDto requestDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(requestDto);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_endpointUrl, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new ApiExceptionResponse((int)response.StatusCode, $"AI service error: {errorContent}");
                }
                var recommendationJson = await response.Content.ReadAsStringAsync();
                var aiResponse = JsonSerializer.Deserialize<AiResponseDto>(recommendationJson);
                if (aiResponse == null)
                {
                    throw new ApiExceptionResponse(500, "Failed to deserialize AI response.");
                }
                return aiResponse;
            }
            catch (HttpRequestException ex)
            {
                // This is thrown if the endpoint is unreachable, DNS fails, etc.
                throw new ApiExceptionResponse(503, $"AI service is unavailable: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // This is thrown if the request times out
                throw new ApiExceptionResponse(504, $"AI service request timed out: {ex.Message}");
            }
        }
    }
} 