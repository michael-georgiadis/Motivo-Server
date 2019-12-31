using Microsoft.EntityFrameworkCore.Migrations;

namespace Motivo.Migrations
{
    public partial class ChangesToGoals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Goals",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumericCurrent",
                table: "Goals",
                maxLength: 256,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumericGoal",
                table: "Goals",
                maxLength: 256,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "NumericCurrent",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "NumericGoal",
                table: "Goals");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
