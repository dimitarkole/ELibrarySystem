using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELibrarySystem.Data.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_GetBooks_GetBookId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_GetBookId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "GetBookId",
                table: "Books");

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedOn",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedOn",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "GetBookId",
                table: "Books",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_GetBookId",
                table: "Books",
                column: "GetBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_GetBooks_GetBookId",
                table: "Books",
                column: "GetBookId",
                principalTable: "GetBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
