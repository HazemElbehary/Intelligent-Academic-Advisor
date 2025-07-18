using FCAI.Domain.Entities;

namespace FCAI.Domain.Extensions
{
    public static class StudentExtensions
    {
        private static readonly IReadOnlyDictionary<string, decimal> GradePoints = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            ["A_plus"] = 4.0m,
            ["A"] = 3.7m,
            ["B_plus"] = 3.3m,
            ["B"] = 3.0m,
            ["C_plus"] = 2.7m,
            ["C"] = 2.4m,
            ["D_plus"] = 2.2m,
            ["D"] = 2.0m,
            ["F"] = 0.0m
        };

        public static decimal CalculateGPA(this Student student)
        {
            // 1. Get all student courses with grades
            var enrollments = student.StudentCourses
                                     .Where(sc => sc.StudentFCAIID == student.FCAIID)
                                     .ToList();

            // Guard against zero courses
            if (!enrollments.Any())
                return 4.0m;

            // 2. Compute total quality points and total credit hours
            decimal totalQualityPoints = 0m;
            int totalCredits = 0;

            foreach (var sc in enrollments)
            {
                // Normalize grade key: strip any trailing '-' if present
                var gradeKey = sc.Grade.ToString().Trim().ToUpperInvariant();
                if (gradeKey.EndsWith("_minus", StringComparison.OrdinalIgnoreCase))
                    gradeKey = gradeKey.Replace("_minus", string.Empty, StringComparison.OrdinalIgnoreCase);

                // Lookup point value (defaults to 0 for unknown)
                if (!GradePoints.TryGetValue(gradeKey, out var point))
                    point = 0m;

                var credits = sc.Course.CreditHours;
                totalQualityPoints += point * credits;
                totalCredits += credits;
            }

            // 3. Divide to get GPA
            var gpa = totalQualityPoints / totalCredits;

            // round to two decimal places
            return Math.Round(gpa, 2, MidpointRounding.AwayFromZero);
        }
    }
}
