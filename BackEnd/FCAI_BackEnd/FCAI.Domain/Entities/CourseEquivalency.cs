using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCAI.Domain.Entities
{
    public class CourseEquivalency
    {
        [Key]
        public required string Code { get; set; }

        [Required]
        public required string Name { get; set; }

        [NotMapped]
        public int CreditHours => CreditCourse?.CreditHours ?? 0;


        // Foreign Key
        public int CreditCourseId { get; set; }

        // Navigation Properties
        [Required]
        public virtual Course CreditCourse { get; set; } = null!;
    }
}
