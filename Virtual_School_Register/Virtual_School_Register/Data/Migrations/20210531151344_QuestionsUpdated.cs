using Microsoft.EntityFrameworkCore.Migrations;

namespace Virtual_School_Register.Data.Migrations
{
    public partial class QuestionsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Test");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Test",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
