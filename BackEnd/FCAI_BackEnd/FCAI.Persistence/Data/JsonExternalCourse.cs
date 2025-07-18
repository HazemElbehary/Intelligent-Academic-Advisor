namespace FCAI.Persistence.Data
{
    public class JsonExternalCourse
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public int CreditHours { get; set; }
        public string? Description { get; set; }
        public int UniversityID { get; set; }
        public required string EquivalentCourseCode { get; set; }

    }
}
