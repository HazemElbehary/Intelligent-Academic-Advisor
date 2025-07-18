using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FCAI.Domain.Entities
{
    public class Department
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public required string Name { get; set; }

        public virtual ICollection<CourseDepartments> CourseDepartments { get; set; }
    }
}
