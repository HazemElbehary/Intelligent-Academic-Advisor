namespace FCAI.Application.Abstraction.DTOs.AI_Response
{
    public class AiResponseDto
    {
        public List<string> core_courses { get; set; }
        public DistributionDto distribution { get; set; }
        public ElectivesDto electives { get; set; }
        public List<IneligibleCourseDto> ineligible_courses { get; set; }
        public OutsideDepartmentCoursesDto outside_dept { get; set; }
        public Dictionary<string, string> remaining_requirements { get; set; }
        public StudentSummaryDto student_summary { get; set; }
        public int total_core_credits { get; set; }
    }

}
