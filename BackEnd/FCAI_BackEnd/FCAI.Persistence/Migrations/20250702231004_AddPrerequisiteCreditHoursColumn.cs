using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPrerequisiteCreditHoursColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrerequisiteDescription",
                table: "CoursePrerequisite");

            migrationBuilder.AddColumn<int>(
                name: "PrerequisiteCreditHours",
                table: "CoursePrerequisite",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrerequisiteCreditHours",
                table: "CoursePrerequisite");

            migrationBuilder.AddColumn<string>(
                name: "PrerequisiteDescription",
                table: "CoursePrerequisite",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
