using Microsoft.EntityFrameworkCore.Migrations;

namespace Virtual_School_Register.Data.Migrations
{
    public partial class UserTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "AspNetUsers");
        }
    }
}
