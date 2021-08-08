using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyAds.WebApp.Data.Migrations
{
    public partial class Watchlists_AddOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Watchlists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_OwnerId",
                table: "Watchlists",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlists_AspNetUsers_OwnerId",
                table: "Watchlists",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Watchlists_AspNetUsers_OwnerId",
                table: "Watchlists");

            migrationBuilder.DropIndex(
                name: "IX_Watchlists_OwnerId",
                table: "Watchlists");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Watchlists");
        }
    }
}
