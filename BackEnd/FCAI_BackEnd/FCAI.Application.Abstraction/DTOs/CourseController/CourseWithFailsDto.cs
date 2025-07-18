namespace FCAI.Application.Abstraction.DTOs.CourseController
{
    public class CourseWithFailsDto
    {
        public required string CourseName { get; set; }
        public int NumberOfFails { get; set; }
    }
}