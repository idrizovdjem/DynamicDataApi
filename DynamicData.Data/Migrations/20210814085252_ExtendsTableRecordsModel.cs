using Microsoft.EntityFrameworkCore.Migrations;

namespace DynamicData.Web.Data.Migrations
{
    public partial class ExtendsTableRecordsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Columns",
                table: "TableRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternalName",
                table: "TableRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Columns",
                table: "TableRecords");

            migrationBuilder.DropColumn(
                name: "InternalName",
                table: "TableRecords");
        }
    }
}
