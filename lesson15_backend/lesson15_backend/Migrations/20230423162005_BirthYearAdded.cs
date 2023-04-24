using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lesson11_backend.Migrations
{
    public partial class BirthYearAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BirtYear",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirtYear",
                table: "Authors");
        }
    }
}
