using FCAI.Application.Abstraction.IServices;
using FCAI.Domain.Contracts;
using FCAI.Domain.Entities;

namespace FCAI.Application.Services.UniversityService
{
    class UniversityService(IUnitOfWork unitOfWork) : IUniversityService
    {
        public async Task<IEnumerable<University>> GetAllAsync()
        {
            var universities = await unitOfWork.GetRepository<University, int>().GetAllAsync();
            return universities;
        }
    }
}
