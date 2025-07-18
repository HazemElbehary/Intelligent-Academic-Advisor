using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTermToFirstAndSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [AspNetUsers] SET [StudentTerm] = 'First' WHERE [StudentTerm] = 'First_Term'");
            migrationBuilder.Sql("UPDATE [AspNetUsers] SET [StudentTerm] = 'Second' WHERE [StudentTerm] = 'Second_Term'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [AspNetUsers] SET [StudentTerm] = 'First_Term' WHERE [StudentTerm] = 'First'");
            migrationBuilder.Sql("UPDATE [AspNetUsers] SET [StudentTerm] = 'Second_Term' WHERE [StudentTerm] = 'Second'");
        }
    }
}
