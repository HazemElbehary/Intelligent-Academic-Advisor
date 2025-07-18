using FCAI.Application.Abstraction.DTOs;

namespace FCAI.Application.Abstraction.IServices
{
    public interface IDepartmentService
    {
        Task<IEnumerable<ReturnedDepartmentDto>> GetAllAsync();
    }
}
