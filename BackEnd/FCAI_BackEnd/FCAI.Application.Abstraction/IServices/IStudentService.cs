using FCAI.Application.Abstraction.DTOs;

namespace FCAI.Application.Abstraction.IServices
{
    public interface IStudentService
    {
        Task<decimal> GetStudentGPAAsync(string? studentId);

        Task<IEnumerable<ReturnedCourseDto>> GetCoursesBasedOnStudentIdAsync(string? FCAIIDClaim, string? universityIdClaim);
        
        Task AddCoursesToStudentAsync(string[] StudentCourses, string[] CourseGrades, string? studentIdClaim);
        
        Task<object> GetRecommendedPlanAsync(int? studentId, string? requestingUserId = null, string? userType = null);

        Task AddDepartmentToStudentAsync(int deparmtentId, string? studentIdCalim);

        Task UpdateTermOfStudentAsync(int termId, string? studentIdCalim);
        void ClearCourseCache();
    }
}
