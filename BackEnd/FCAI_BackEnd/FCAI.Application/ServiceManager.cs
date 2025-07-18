using FCAI.Application.Abstraction;
using FCAI.Application.Abstraction.IServices;

namespace FCAI.Application
{
    public class ServiceManager : IServiceManagers
    {
        private readonly Lazy<IStudentService> _studentService;
        private readonly Lazy<IUniversityService> _universityService;
        private readonly Lazy<IDepartmentService> _departmentService;
        private readonly Lazy<ICourseService> _courseService;
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<ITermService> _termService;

        public ServiceManager(
            IStudentService StudentService,
            IUniversityService UniversityService,
            IDepartmentService DepartmentService,
            ICourseService CourseService,
            IAuthService AuthService,
            ITermService TermService)
        {
            _studentService = new Lazy<IStudentService>(StudentService);
            _universityService = new Lazy<IUniversityService>(UniversityService);
            _departmentService = new Lazy<IDepartmentService>(DepartmentService);
            _courseService = new Lazy<ICourseService>(CourseService);
            _authService = new Lazy<IAuthService>(AuthService);
            _termService = new Lazy<ITermService>(TermService);
        }
        public IStudentService StudentService => _studentService.Value;
        public IUniversityService UniversityService => _universityService.Value;
        public IDepartmentService DepartmentService => _departmentService.Value;
        public ICourseService CourseService => _courseService.Value;
        public IAuthService AuthService => _authService.Value;
        public ITermService TermService => _termService.Value;
    }
}
