using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.DAL.Migrations
{
    public partial class Seeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "rating",
                columns: new[] { "Id", "ApplicationUserId", "FitnessPlanId", "text", "value" },
                values: new object[] { 1, null, null, null, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "rating",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
