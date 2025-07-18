using FCAI.Domain.Enums;

namespace FCAI.Domain.Entities
{
    public class FCAICourses: Course
    {
        public CourseDistributionCategories DistributionCategory { get; set; }

        public CourseTypes Type { get; set; }

        public Terms? Term { get; set; }



        // Navigation Properties
        public virtual ICollection<CoursePrerequisite> Prerequisites { get; set; } = new HashSet<CoursePrerequisite>();
        public virtual ICollection<CourseDepartments> CourseDepartments { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
