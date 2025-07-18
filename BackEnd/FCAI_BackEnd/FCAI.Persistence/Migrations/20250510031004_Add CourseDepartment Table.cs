using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseDepartmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseDepartments",
                columns: table => new
                {
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDepartments", x => new { x.CourseCode, x.DepartmentID });
                    table.ForeignKey(
                        name: "FK_CourseDepartments_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDepartments_FCAICourses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "FCAICourses",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDepartments_CourseID_DepartmentID",
                table: "CourseDepartments",
                columns: new[] { "CourseCode", "DepartmentID" });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDepartments_DepartmentID_CourseID",
                table: "CourseDepartments",
                columns: new[] { "DepartmentID", "CourseCode" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseDepartments");
        }
    }
}
