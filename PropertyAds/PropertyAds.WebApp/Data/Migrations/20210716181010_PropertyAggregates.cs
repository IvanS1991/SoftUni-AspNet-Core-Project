using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyAds.WebApp.Data.Migrations
{
    public partial class PropertyAggregates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PropertyAggregates",
                columns: table => new
                {
                    DistrictId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AveragePrice = table.Column<int>(type: "int", nullable: false),
                    AveragePricePerSqM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyAggregates", x => new { x.DistrictId, x.PropertyTypeId });
                    table.ForeignKey(
                        name: "FK_PropertyAggregates_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyAggregates_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAggregates_PropertyTypeId",
                table: "PropertyAggregates",
                column: "PropertyTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyAggregates");
        }
    }
}
