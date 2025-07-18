using FCAI.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCAI.Domain.Entities
{
    public class StudentCourse
    {
        public required Grades Grade { get; set; }
        public int StudentFCAIID { get; set; }
        public string CourseCode { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(StudentFCAIID))]
        public virtual Student Student { get; set; } = null!;

        [ForeignKey(nameof(CourseCode))]
        public virtual FCAICourses Course { get; set; } = null!;
    }
}
