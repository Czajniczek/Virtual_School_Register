using Microsoft.EntityFrameworkCore.Migrations;

namespace Virtual_School_Register.Data.Migrations
{
    public partial class TestUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_ConductingLesson_ConductingLessonId",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ClassSubjectTeacherId",
                table: "Test");

            migrationBuilder.AlterColumn<int>(
                name: "ConductingLessonId",
                table: "Test",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_ConductingLesson_ConductingLessonId",
                table: "Test",
                column: "ConductingLessonId",
                principalTable: "ConductingLesson",
                principalColumn: "ConductingLessonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_ConductingLesson_ConductingLessonId",
                table: "Test");

            migrationBuilder.AlterColumn<int>(
                name: "ConductingLessonId",
                table: "Test",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ClassSubjectTeacherId",
                table: "Test",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_ConductingLesson_ConductingLessonId",
                table: "Test",
                column: "ConductingLessonId",
                principalTable: "ConductingLesson",
                principalColumn: "ConductingLessonId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
