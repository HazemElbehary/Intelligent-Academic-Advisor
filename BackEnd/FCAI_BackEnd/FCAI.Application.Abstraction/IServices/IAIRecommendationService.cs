using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.DTOs.AI_Response;
using System.Threading.Tasks;

namespace FCAI.Application.Abstraction.IServices
{
    public interface IAIRecommendationService
    {
        Task<AiResponseDto> GetRecommendationAsync(AiRequestDto requestDto);
    }
} 