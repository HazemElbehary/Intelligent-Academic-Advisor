using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class storegradesasstringinDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameTable(
                name: "CK_StudentCourse_Grade_Range",
                schema: "Grade BETWEEN 0 AND 100",
                newName: "StudentCourses");

            migrationBuilder.RenameIndex(
                name: "IX_CK_StudentCourse_Grade_Range_CourseCode",
                table: "StudentCourses",
                newName: "IX_StudentCourses_CourseCode");

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "StudentCourses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                schema: "Grade BETWEEN 0 AND 100",
                table: "CK_StudentCourse_Grade_Range",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
    }
}
