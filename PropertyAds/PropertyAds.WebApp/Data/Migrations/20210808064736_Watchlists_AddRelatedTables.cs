using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyAds.WebApp.Data.Migrations
{
    public partial class Watchlists_AddRelatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Properties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Watchlists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastViewedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchlistProperties",
                columns: table => new
                {
                    WatchlistId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchlistProperties", x => new { x.WatchlistId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_WatchlistProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchlistProperties_Watchlists_WatchlistId",
                        column: x => x.WatchlistId,
                        principalTable: "Watchlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchlistPropertySegments",
                columns: table => new
                {
                    WatchlistId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DistrictId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchlistPropertySegments", x => new { x.WatchlistId, x.PropertyTypeId, x.DistrictId });
                    table.ForeignKey(
                        name: "FK_WatchlistPropertySegments_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchlistPropertySegments_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchlistPropertySegments_Watchlists_WatchlistId",
                        column: x => x.WatchlistId,
                        principalTable: "Watchlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistProperties_PropertyId",
                table: "WatchlistProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistPropertySegments_DistrictId",
                table: "WatchlistPropertySegments",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistPropertySegments_PropertyTypeId",
                table: "WatchlistPropertySegments",
                column: "PropertyTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchlistProperties");

            migrationBuilder.DropTable(
                name: "WatchlistPropertySegments");

            migrationBuilder.DropTable(
                name: "Watchlists");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Properties");
        }
    }
}
