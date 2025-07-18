using System.ComponentModel.DataAnnotations.Schema;

namespace FCAI.Domain.Entities
{
    public class CoursePrerequisite
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = null!;

        public string? PrerequisiteCourseCode { get; set; } = null!;


        // if this prereq is a text rule "Passing 30 Credit Hours":
        public int PrerequisiteCreditHours { get; set; }


        // Navigation properties (required)
        [ForeignKey(nameof(CourseCode))]
        [InverseProperty(nameof(FCAICourses.Prerequisites))]
        public virtual FCAICourses Course { get; set; } = null!;

        [ForeignKey(nameof(PrerequisiteCourseCode))]
        public virtual FCAICourses? PrerequisiteCourse { get; set; } = null!;
    }
}
