using FCAI.Application.Abstraction;
using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.IServices;
using FCAI.Application.Services.CourseServices;
using FCAI.Application.Services.DeptService;
using FCAI.Application.Services.StudentServices;
using FCAI.Application.Services.UniversityService;
using FCAI.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCAI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped(typeof(IServiceManagers), typeof(ServiceManager));
            service.AddScoped(typeof(IStudentService), typeof(StudentService));
            service.AddScoped(typeof(IUniversityService), typeof(UniversityService));
            service.AddScoped(typeof(IDepartmentService), typeof(DepartmentService));
            service.AddScoped(typeof(ICourseService), typeof(CourseService));
            service.AddScoped(typeof(IAuthService), typeof(AuthService));
            service.AddScoped(typeof(ITermService), typeof(TermService));
            service.AddHttpClient<IAIRecommendationService, AIRecommendationService>();
            service.AddMemoryCache();

            // Configure JwtSettings
            service.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            return service;
        }
    }
}
