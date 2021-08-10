using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyAds.WebApp.Data.Migrations
{
    public partial class Conversation_AddProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyId",
                table: "Conversations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_PropertyId",
                table: "Conversations",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Properties_PropertyId",
                table: "Conversations",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Properties_PropertyId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_PropertyId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Conversations");
        }
    }
}
