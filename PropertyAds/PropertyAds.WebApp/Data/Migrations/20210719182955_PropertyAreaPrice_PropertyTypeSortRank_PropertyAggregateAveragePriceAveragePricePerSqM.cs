using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyAds.WebApp.Data.Migrations
{
    public partial class PropertyAreaPrice_PropertyTypeSortRank_PropertyAggregateAveragePriceAveragePricePerSqM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortRank",
                table: "PropertyTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AveragePricePerSqM",
                table: "PropertyAggregates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AveragePrice",
                table: "PropertyAggregates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Properties",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<int>(
                name: "Area",
                table: "Properties",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortRank",
                table: "PropertyTypes");

            migrationBuilder.AlterColumn<int>(
                name: "AveragePricePerSqM",
                table: "PropertyAggregates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AveragePrice",
                table: "PropertyAggregates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Properties",
                type: "money",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Area",
                table: "Properties",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
