using FCAI.Domain.Entities;
using FCAI.Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FCAI.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<Student>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ── TPC setup for your Course hierarchy ────────────────────────────────
            builder.Entity<Course>().UseTpcMappingStrategy();
            builder.Entity<FCAICourses>().ToTable("FCAICourses");
            builder.Entity<ExternalCourses>().ToTable("ExternalCourses");

            // ── Student ───────────────────────────────────────────
            builder.Entity<Student>(s =>
            {
                s.HasAlternateKey(x => x.FCAIID);
                s.Property(x => x.StudentTerm)
                  .HasConversion(
                  v => v.ToString(),
                  v => (StudentTerm)Enum.Parse(typeof(StudentTerm), v));
                
                s.Property(x => x.UserType)
                  .HasConversion<int>();
            });

            // ──  StudentCourse ─────────────────
            builder.Entity<StudentCourse>(sc =>
            {
                sc.HasKey(x => new { x.StudentFCAIID, x.CourseCode });
                // StudentCourse.StudentFCAIID → Student.FCAIID
                sc.HasOne(x => x.Student)
                  .WithMany(s => s.StudentCourses)
                  .HasForeignKey(x => x.StudentFCAIID)
                  .HasPrincipalKey(s => s.FCAIID);

                // StudentCourse.CourseCode → Course.Code (principal key on base Course)
                sc.HasOne(x => x.Course)
                  .WithMany(c => c.StudentCourses)
                  .HasForeignKey(x => x.CourseCode)
                  .HasPrincipalKey(c => c.Code);

                sc.Property(x => x.Grade)
                  .HasConversion(
                  v => v.ToString(),
                  v => (Grades)Enum.Parse(typeof(Grades), v));
            });

            // ── configure course prerequisites relationship ──────────────────────────
            builder.Entity<CoursePrerequisite>(cp =>
            {
                cp.HasOne(e => e.Course)
                  .WithMany(c => c.Prerequisites)
                  .HasForeignKey(e => e.CourseCode)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            // ── ExternalCourses self‑reference ───────────────────────────────────────
            builder.Entity<ExternalCourses>(ec =>
            {
                ec.HasOne(c => c.EquivalentCourse)
                  .WithMany()
                  .HasForeignKey(c => c.EquivalentCourseCode)
                  .OnDelete(DeleteBehavior.Restrict);

                ec.HasOne(c => c.UniversityCourse)
                  .WithMany()
                  .HasForeignKey(c => c.UniversityID)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Enum conversions on FCAICourses ─────────────────────────────────────
            builder.Entity<FCAICourses>(fc =>
            {
                fc.Property(e => e.DistributionCategory)
                  .HasConversion(
                      v => v.ToString(),
                      v => (CourseDistributionCategories)Enum.Parse(typeof(CourseDistributionCategories), v)
                  )
                  .HasMaxLength(50)
                  .IsRequired();

                fc.Property(e => e.Type)
                  .HasConversion<string>()
                  .HasMaxLength(50);

                fc.Property(e => e.Term)
                  .HasConversion(
                      v => v.ToString(),
                      v => (Terms)Enum.Parse(typeof(Terms), v)
                  )
                  .HasMaxLength(50);
            });

            // ── configure course departments relationship ─────────────────────────────────
            builder.Entity<CourseDepartments>(entity =>
            {
                // Composite primary key
                entity.HasKey(cd => new { cd.CourseCode, cd.DepartmentID });

                // Index to speed up: "Which departments belong to a course?"
                entity.HasIndex(cd => new { cd.CourseCode, cd.DepartmentID })
                      .HasDatabaseName("IX_CourseDepartments_CourseID_DepartmentID");

                // Index to speed up: "Which courses belong to a department?"
                entity.HasIndex(cd => new { cd.DepartmentID, cd.CourseCode })
                      .HasDatabaseName("IX_CourseDepartments_DepartmentID_CourseID");



                // relationships
                entity.HasOne(cd => cd.Department)
                      .WithMany(d => d.CourseDepartments)
                      .HasForeignKey(cd => cd.DepartmentID);

                entity.HasOne(cd => cd.FCAICourses)
                      .WithMany(c => c.CourseDepartments)
                      .HasForeignKey(cd => cd.CourseCode);
            });
        }

        public required DbSet<Student> Students { get; set; }
        public required DbSet<University> Universities { get; set; }
        public required DbSet<Department> Departments { get; set; }
        public required DbSet<StudentCourse> StudentCourses { get; set; }
        public required DbSet<FCAICourses> FCAICourses { get; set; }
        public required DbSet<ExternalCourses> ExternalCourses { get; set; }

    }
}
