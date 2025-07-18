using FCAI.Application.Abstraction.DTOs;

namespace FCAI.Application.Abstraction.IServices
{
    public interface ITermService
    {
        Task<IEnumerable<ReturnedTermDto>> GetAllAsync();
    }
}
