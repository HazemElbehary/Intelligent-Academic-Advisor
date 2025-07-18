namespace FCAI.Application.Abstraction.DTOs
{
    public class ReturnedCourseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }

        // FCAI‑only fields
        public string? DistributionCategory { get; set; }
        public string? Type { get; set; }
        public string? Term { get; set; }

        // External‑only fields
        public string? EquivalentCourseCode { get; set; }
    }
}