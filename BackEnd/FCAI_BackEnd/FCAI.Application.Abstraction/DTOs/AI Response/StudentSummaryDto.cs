namespace FCAI.Application.Abstraction.DTOs.AI_Response
{
    public class StudentSummaryDto
    {
        public string AcademicLevel { get; set; }
        public int CreditLimit { get; set; }
        public string CurrentTerm { get; set; }
        public string Department { get; set; }
        public decimal Gpa { get; set; }
        public int StudentId { get; set; }
        public int TotalCompletedHours { get; set; }
        public int TotalRemaining { get; set; }
    }
}
