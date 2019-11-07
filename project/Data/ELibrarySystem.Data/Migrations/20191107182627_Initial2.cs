using Microsoft.EntityFrameworkCore.Migrations;

namespace ELibrarySystem.Data.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_GetBooks_GetBookId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_ApplicationUserId1",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ApplicationUserId1",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GetBookId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "GetBookId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GetBooks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GetBooks_UserId",
                table: "GetBooks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GetBooks_AspNetUsers_UserId",
                table: "GetBooks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetBooks_AspNetUsers_UserId",
                table: "GetBooks");

            migrationBuilder.DropIndex(
                name: "IX_GetBooks_UserId",
                table: "GetBooks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GetBooks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GetBookId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_ApplicationUserId1",
                table: "Books",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GetBookId",
                table: "AspNetUsers",
                column: "GetBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GetBooks_GetBookId",
                table: "AspNetUsers",
                column: "GetBookId",
                principalTable: "GetBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_ApplicationUserId1",
                table: "Books",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
