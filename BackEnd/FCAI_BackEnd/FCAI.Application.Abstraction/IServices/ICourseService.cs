using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.DTOs.CourseController;

namespace FCAI.Application.Abstraction.IServices
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseWithFailsDto>> GetCoursesWithFailsAsync();
    }
}
