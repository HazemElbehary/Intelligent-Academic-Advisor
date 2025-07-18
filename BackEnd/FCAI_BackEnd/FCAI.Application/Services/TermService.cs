using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.IServices;
using FCAI.Domain.Contracts;
using FCAI.Domain.Entities;
using FCAI.Domain.Enums;

namespace FCAI.Application.Services.DeptService
{
    class TermService(IUnitOfWork unitOfWork): ITermService
    {
        public async Task<IEnumerable<ReturnedTermDto>> GetAllAsync()
        {
            var terms = Enum.GetValues(typeof(StudentTerm))
                            .Cast<StudentTerm>()
                            .Select(t => new ReturnedTermDto { Id = (int)t, Name = t.ToString() })
                            .ToList();
            return terms;
        }
    }
}
