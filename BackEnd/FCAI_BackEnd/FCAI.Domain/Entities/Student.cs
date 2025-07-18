using FCAI.Domain.Enums;
using FCAI.Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCAI.Domain.Entities
{
    public class Student : IdentityUser
    {
        [Key]
        public int FCAIID { get; set; }

        [NotMapped]
        public decimal GPA
        {
            get { return this.CalculateGPA(); }
            private set { }
        }

        public UserType UserType { get; set; } = UserType.Student;
        
        // Student-specific properties (nullable for admin users)
        public StudentTerm? StudentTerm { get; set; }
        public int? DepartmentId { get; set; }
        public int? UniversityId { get; set; }

        // Admin-specific properties (nullable for student users)
        public string? FullName { get; set; }
        public string? Position { get; set; }
        public DateTime? HireDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey(nameof(UniversityId))]
        public virtual University? StudentUniversity { get; set; }
        
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();
        public virtual Department? Department { get; set; }
    }
}