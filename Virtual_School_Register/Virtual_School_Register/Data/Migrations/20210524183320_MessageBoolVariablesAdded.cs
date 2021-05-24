using Microsoft.EntityFrameworkCore.Migrations;

namespace Virtual_School_Register.Data.Migrations
{
    public partial class MessageBoolVariablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecipientDeleted",
                table: "Message",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSenderDeleted",
                table: "Message",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecipientDeleted",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsSenderDeleted",
                table: "Message");
        }
    }
}
