using System.ComponentModel.DataAnnotations;

namespace FCAI.Application.Abstraction.DTOs
{
    public class RegisterDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "UserName must contain only letters, numbers, and underscores.")]
        public required string UserName { get; set; }

        [Required]
        [RegularExpression(@"^202\d{5}$", ErrorMessage = "ID must be FCAI ID.")]
        public required int FCAIID { get; set; }

        public required string Password { get; set; }
        public required int UniversityId { get; set; }
        public int? DepartmentId { get; set; }
        public int UserTerm { get; set; }
    }
}
