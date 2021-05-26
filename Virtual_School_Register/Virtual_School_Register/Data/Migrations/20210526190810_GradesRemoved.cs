using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Virtual_School_Register.Data.Migrations
{
    public partial class GradesRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_Grade_GradeId",
                table: "Evaluation");

            migrationBuilder.DropIndex(
                name: "IX_Evaluation_GradeId",
                table: "Evaluation");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Evaluation");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Evaluation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Evaluation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Evaluation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Value",
                table: "Evaluation",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Evaluation");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Evaluation");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Evaluation");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Evaluation");

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "Evaluation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluation_GradeId",
                table: "Evaluation",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_Grade_GradeId",
                table: "Evaluation",
                column: "GradeId",
                principalTable: "Grade",
                principalColumn: "GradeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
