using Microsoft.EntityFrameworkCore.Migrations;

namespace ELibrarySystem.Data.Migrations
{
    public partial class AddCatalogNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CatalogNumber",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Commentar",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatalogNumber",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Commentar",
                table: "Books");
        }
    }
}
