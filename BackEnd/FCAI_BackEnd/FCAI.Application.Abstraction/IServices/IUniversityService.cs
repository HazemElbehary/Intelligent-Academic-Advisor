using FCAI.Domain.Entities;

namespace FCAI.Application.Abstraction.IServices
{
    public interface IUniversityService
    {
        Task<IEnumerable<University>> GetAllAsync();
    }
}
