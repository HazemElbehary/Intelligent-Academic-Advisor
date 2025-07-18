using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCAI.Domain.Entities
{
    public class ExternalCourses: Course
    {
        // Foreign Key
        public int UniversityID { get; set; }

        [Required]
        public required string EquivalentCourseCode { get; set; }

        // Navigation Properties
        [Required]
        public virtual University UniversityCourse { get; set; }
        
        [ForeignKey(nameof(EquivalentCourseCode))]
        public virtual FCAICourses EquivalentCourse { get; set; }

    }
}
