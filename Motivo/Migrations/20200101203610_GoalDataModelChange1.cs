using Microsoft.EntityFrameworkCore.Migrations;

namespace Motivo.Migrations
{
    public partial class GoalDataModelChange1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Goals");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Goals",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Goals");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Goals",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Goals",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }
    }
}
