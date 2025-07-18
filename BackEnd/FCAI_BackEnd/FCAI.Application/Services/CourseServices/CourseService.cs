using FCAI.Application.Abstraction.DTOs.CourseController;
using FCAI.Application.Abstraction.IServices;
using FCAI.Domain.Contracts;
using FCAI.Domain.Entities;
using FCAI.Domain.Enums;
using FCAI.Domain.Specifications;

namespace FCAI.Application.Services.CourseServices
{
    class CourseService(IUnitOfWork _unitOfWork) : ICourseService
    {
        public async Task<IEnumerable<CourseWithFailsDto>> GetCoursesWithFailsAsync()
        {
            var courses = await _unitOfWork.GetRepository<StudentCourse, string>().GetAllWithSpecAsync(new BaseISpecifications<StudentCourse, string>(sc => sc.Grade == Grades.F));
            var grouped = courses.GroupBy(sc => sc.CourseCode);

            var result = new List<CourseWithFailsDto>();
            foreach (var g in grouped)
            {
                var course = await _unitOfWork.GetRepository<Course, string>()
                    .GetWithSpecAsync(new BaseISpecifications<Course, string>(c => c.Code == g.Key));
                result.Add(new CourseWithFailsDto
                {
                    CourseName = course!.Name,
                    NumberOfFails = g.Count()
                });
            }
            return result;
        }
    }
}