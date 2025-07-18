namespace FCAI.Application.Abstraction.DTOs.AI_Response
{
    public class OutsideDepartmentCoursesDto
    {
        public List<string> AvailableOutsideCoursesCodes { get; set; }
        public int CoursesCodesCanTakeOutside { get; set; }
        public int NumOutsideDeptTakenCourses { get; set; }
    }
}
