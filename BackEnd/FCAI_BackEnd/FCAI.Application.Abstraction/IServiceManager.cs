using FCAI.Application.Abstraction.IServices;

namespace FCAI.Application.Abstraction
{
    public interface IServiceManagers
    {
        public IStudentService StudentService { get; }
        public IUniversityService UniversityService { get; }
        public IDepartmentService DepartmentService { get; }
        public ITermService TermService { get; }
        public ICourseService CourseService { get; }
        public IAuthService AuthService { get; }
    }
}
