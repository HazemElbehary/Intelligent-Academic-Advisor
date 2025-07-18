using System.ComponentModel.DataAnnotations;

namespace FCAI.Domain.Entities
{
    public abstract class Course
    {
        [Key]
        public required string Code { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public int CreditHours { get; set; }

        public string? Description { get; set; }
    }
}
