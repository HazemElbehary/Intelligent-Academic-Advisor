namespace FCAI.Application.Abstraction.DTOs.CourseController
{
    public class AIReturnedCourseDto
    {
        public required string code { get; set; }
        public required string course_name { get; set; }
        public int credit_hours { get; set; }
        public required string distribution_category { get; set; }
        public required string type { get; set; }
        public string? Term { get; set; }
        public required string department { get; set; }
        public List<string> prerequisites { get; set; } = new List<string>();
    }
}