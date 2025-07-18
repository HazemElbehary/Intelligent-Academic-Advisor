using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIdprimaryKeytoCoursePrerequisite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrerequisite_FCAICourses_PrerequisiteCourseCode",
                table: "CoursePrerequisite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoursePrerequisite",
                table: "CoursePrerequisite");

            migrationBuilder.AlterColumn<string>(
                name: "PrerequisiteCourseCode",
                table: "CoursePrerequisite",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CoursePrerequisite",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoursePrerequisite",
                table: "CoursePrerequisite",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePrerequisite_CourseCode",
                table: "CoursePrerequisite",
                column: "CourseCode");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrerequisite_FCAICourses_PrerequisiteCourseCode",
                table: "CoursePrerequisite",
                column: "PrerequisiteCourseCode",
                principalTable: "FCAICourses",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePrerequisite_FCAICourses_PrerequisiteCourseCode",
                table: "CoursePrerequisite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoursePrerequisite",
                table: "CoursePrerequisite");

            migrationBuilder.DropIndex(
                name: "IX_CoursePrerequisite_CourseCode",
                table: "CoursePrerequisite");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CoursePrerequisite");

            migrationBuilder.AlterColumn<string>(
                name: "PrerequisiteCourseCode",
                table: "CoursePrerequisite",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoursePrerequisite",
                table: "CoursePrerequisite",
                columns: new[] { "CourseCode", "PrerequisiteCourseCode" });

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePrerequisite_FCAICourses_PrerequisiteCourseCode",
                table: "CoursePrerequisite",
                column: "PrerequisiteCourseCode",
                principalTable: "FCAICourses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
