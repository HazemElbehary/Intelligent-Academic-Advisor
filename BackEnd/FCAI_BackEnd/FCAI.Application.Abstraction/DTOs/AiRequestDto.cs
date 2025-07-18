using FCAI.Application.Abstraction.DTOs.CourseController;

namespace FCAI.Application.Abstraction.DTOs
{
    public class AiRequestDto
    {
        public int StudentId { get; set; }
        public string? DepartmentName { get; set; }
        public string Term { get; set; }
        public decimal GPA { get; set; }
        public List<AIReturnedCourseDto> CompletedCourses { get; set; }
        public List<AIReturnedCourseDto> AllCourses { get; set; }
    }
}
