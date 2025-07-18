using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExternalCoursesStudentCoursesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_FCAICourses_CourseCode",
                table: "StudentCourses",
                column: "CourseCode",
                principalTable: "FCAICourses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_FCAICourses_CourseCode",
                table: "StudentCourses");
        }
    }
}
