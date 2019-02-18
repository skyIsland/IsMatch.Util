using Microsoft.EntityFrameworkCore.Migrations;

namespace IsMatch.Bill.WebApi.Migrations
{
    public partial class addfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsDelete",
                table: "BillInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "BillInfo");
        }
    }
}
