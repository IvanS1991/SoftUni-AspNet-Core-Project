using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyAds.WebApp.Data.Migrations
{
    public partial class Conversation_AddRecipient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "Conversations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_RecipientId",
                table: "Conversations",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_AspNetUsers_RecipientId",
                table: "Conversations",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_AspNetUsers_RecipientId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_RecipientId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Conversations");
        }
    }
}
