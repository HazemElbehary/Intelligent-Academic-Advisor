using FCAI.Domain.Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using FCAI.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace FCAI.Persistence.Data
{
    public class ApplicationInitializer(
        ApplicationDbContext _dbContext,
        UserManager<Student> _userManager,
        RoleManager<IdentityRole> _roleManager)
    {
        public async Task SeedAsync()
        {
            // 0. Seed Roles
            await SeedRolesAsync();

            // 1. Seed Admin Users
            await SeedAdminUsersAsync();

            // 2. Seed Universities
            if (!await _dbContext.Universities.AnyAsync())
            {
                _dbContext.Universities.AddRange(new[]
                {
                    new University { Name = "Credit Cairo University" },
                    new University { Name = "General Cairo University" },
                    new University { Name = "Assiut University" },
                    new University { Name = "Mansoura University" },
                    new University { Name = "Tanta University" },
                    new University { Name = "Sohag University" },
                    new University { Name = "Minia University" },
                    new University { Name = "Benha University" },
                    new University { Name = "Luxor University" },
                    new University { Name = "Ain Shams University" },
                    new University { Name = "Fayoum University" }
                });
            }

            // 3. Seed Departments
            if (!await _dbContext.Departments.AnyAsync())
            {
                _dbContext.Departments.AddRange(new[]
                {
                    new Department { Name = "AI" },
                    new Department { Name = "CS" },
                    new Department { Name = "DS" },
                    new Department { Name = "IS" },
                    new Department { Name = "IT" },
                });
            }

            // 4. Load JSON DTOs
            var coursesJson = await File.ReadAllTextAsync("../FCAI.Persistence/Data/Courses_v2.json");
            var coursesDto = JsonSerializer.Deserialize<List<JsonCourse>>(coursesJson)!;

            var externalJson = await File.ReadAllTextAsync("../FCAI.Persistence/Data/ExternalCourses.json");
            var externalDto = JsonSerializer.Deserialize<List<JsonExternalCourse>>(externalJson)!;

            // 5. Seed FCAICourses
            if (!await _dbContext.FCAICourses.AnyAsync())
            {
                var courses = coursesDto.Select(json => new FCAICourses
                {
                    Code = json.code,
                    Name = json.course_name,
                    CreditHours = json.credit_hours,
                    Type = Enum.Parse<CourseTypes>(json.type, true),
                    Term = json.Term == "null" ? null : Enum.Parse<Terms>(json.Term, true),
                    DistributionCategory = Enum.Parse<CourseDistributionCategories>(json.distribution_category, true)
                }).ToList();

                _dbContext.FCAICourses.AddRange(courses);
            }

            // 6. Seed CourseDepartments (many-to-many)
            if (!await _dbContext.Set<CourseDepartments>().AnyAsync())
            {
                var courseDeps = coursesDto
                    .Where(c => c.department != "null")
                    .Select(c => new CourseDepartments
                    {
                        CourseCode = c.code,
                        DepartmentID = (int)Enum.Parse<DepartmentEnum>(c.department, true)
                    })
                    .ToList();

                _dbContext.Set<CourseDepartments>().AddRange(courseDeps);
            }

            // 7. Seed CoursePrerequisite (self-referencing)
            if (!await _dbContext.Set<CoursePrerequisite>().AnyAsync())
            {
                var knownCodes = coursesDto.Select(c => c.code).ToHashSet(StringComparer.OrdinalIgnoreCase);

                var prereqs = coursesDto
                    .SelectMany(c => c.prerequisites, (c, p) => new { c.code, prereq = p })
                    .Select(x => new CoursePrerequisite
                    {
                        CourseCode = x.code,
                        PrerequisiteCourseCode = knownCodes.Contains(x.prereq) ? x.prereq : null,
                        PrerequisiteCreditHours = knownCodes.Contains(x.prereq) ? 0 : int.Parse(Regex.Match(x.prereq, @"\d+").Value)
                    })
                    .GroupBy(pr => new { pr.CourseCode, pr.PrerequisiteCourseCode, pr.PrerequisiteCreditHours })
                    .Select(g => g.First())
                    .ToList();

                _dbContext.Set<CoursePrerequisite>().AddRange(prereqs);
            }

            // 8. Seed ExternalCourses
            if (!await _dbContext.ExternalCourses.AnyAsync())
            {
                var externals = externalDto.Select(json => new ExternalCourses
                {
                    Code = json.Code,
                    Name = json.Name,
                    CreditHours = json.CreditHours,
                    Description = json.Description,
                    UniversityID = json.UniversityID,
                    EquivalentCourseCode = json.EquivalentCourseCode
                }).ToList();

                _dbContext.ExternalCourses.AddRange(externals);
            }

            // 9. Save all changes in one transaction
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task SeedRolesAsync()
        {
            var roles = new[] { "Student", "Admin" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedAdminUsersAsync()
        {
            // Check if admin already exists
            var existingAdmin = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserType == UserType.Admin);

            if (existingAdmin == null)
            {
                var admin = new Student
                {
                    FCAIID = 999999, // Special admin ID
                    UserName = "admin",
                    UserType = UserType.Admin,
                    FullName = "System Administrator",
                    Position = "System Administrator",
                    HireDate = DateTime.UtcNow,
                    IsActive = true,
                    Email = "admin@fcai.com",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(admin, "Admin123!");
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
