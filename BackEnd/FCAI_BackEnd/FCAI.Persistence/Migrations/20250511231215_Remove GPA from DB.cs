using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGPAfromDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_AspNetUsers_StudentFCAIID",
                table: "StudentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_FCAICourses_CourseCode",
                table: "StudentCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "GPA",
                table: "AspNetUsers");

            migrationBuilder.EnsureSchema(
                name: "Grade BETWEEN 0 AND 100");

            migrationBuilder.RenameTable(
                name: "StudentCourses",
                newName: "CK_StudentCourse_Grade_Range",
                newSchema: "Grade BETWEEN 0 AND 100");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_CourseCode",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range",
                newName: "IX_CK_StudentCourse_Grade_Range_CourseCode");

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CK_StudentCourse_Grade_Range",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range",
                columns: new[] { "StudentFCAIID", "CourseCode" });

            migrationBuilder.AddForeignKey(
                name: "FK_CK_StudentCourse_Grade_Range_AspNetUsers_StudentFCAIID",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range",
                column: "StudentFCAIID",
                principalTable: "AspNetUsers",
                principalColumn: "FCAIID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CK_StudentCourse_Grade_Range_FCAICourses_CourseCode",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range",
                column: "CourseCode",
                principalTable: "FCAICourses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CK_StudentCourse_Grade_Range_AspNetUsers_StudentFCAIID",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range");

            migrationBuilder.DropForeignKey(
                name: "FK_CK_StudentCourse_Grade_Range_FCAICourses_CourseCode",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CK_StudentCourse_Grade_Range",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range");

            migrationBuilder.DropColumn(
                name: "Grade",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range");

            migrationBuilder.RenameTable(
                name: "CK_StudentCourse_Grade_Range",
                schema: "Grade BETWEEN 0 AND 100",
                newName: "StudentCourses");

            migrationBuilder.RenameIndex(
                name: "IX_CK_StudentCourse_Grade_Range_CourseCode",
                table: "StudentCourses",
                newName: "IX_StudentCourses_CourseCode");

            migrationBuilder.AddColumn<decimal>(
                name: "GPA",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses",
                columns: new[] { "StudentFCAIID", "CourseCode" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_AspNetUsers_StudentFCAIID",
                table: "StudentCourses",
                column: "StudentFCAIID",
                principalTable: "AspNetUsers",
                principalColumn: "FCAIID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_FCAICourses_CourseCode",
                table: "StudentCourses",
                column: "CourseCode",
                principalTable: "FCAICourses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
