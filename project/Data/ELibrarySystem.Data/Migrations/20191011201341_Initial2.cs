using Microsoft.EntityFrameworkCore.Migrations;

namespace ELibrarySystem.Data.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BookId",
                table: "GetBooks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GetBooks_BookId",
                table: "GetBooks",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_GetBooks_Books_BookId",
                table: "GetBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetBooks_Books_BookId",
                table: "GetBooks");

            migrationBuilder.DropIndex(
                name: "IX_GetBooks_BookId",
                table: "GetBooks");

            migrationBuilder.AlterColumn<string>(
                name: "BookId",
                table: "GetBooks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
