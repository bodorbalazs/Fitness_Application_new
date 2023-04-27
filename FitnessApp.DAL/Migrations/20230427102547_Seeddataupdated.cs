using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.DAL.Migrations
{
    public partial class Seeddataupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "rating",
                keyColumn: "Id",
                keyValue: 1,
                column: "value",
                value: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "rating",
                keyColumn: "Id",
                keyValue: 1,
                column: "value",
                value: 1);
        }
    }
}
