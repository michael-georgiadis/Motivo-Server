using Microsoft.EntityFrameworkCore.Migrations;

namespace Motivo.Migrations
{
    public partial class GoalDataModelChange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddBy",
                table: "Goals",
                maxLength: 20,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddBy",
                table: "Goals");
        }
    }
}
