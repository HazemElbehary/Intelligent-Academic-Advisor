using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.IServices;
using FCAI.Domain.Contracts;
using FCAI.Domain.Entities;

namespace FCAI.Application.Services.DeptService
{
    class DepartmentService(IUnitOfWork unitOfWork): IDepartmentService
    {
        public async Task<IEnumerable<ReturnedDepartmentDto>> GetAllAsync()
        {
            var departments = await unitOfWork.GetRepository<Department, int>().GetAllAsync();
            return departments.Select(d => new ReturnedDepartmentDto() {Id = d.ID, Name = d.Name });
        }
    }
}
