using System.ComponentModel.DataAnnotations;

namespace FCAI.Domain.Entities
{
    public class CourseDepartments
    {
        public string CourseCode { get; set; }
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public virtual FCAICourses FCAICourses { get; set; }
    }
}
