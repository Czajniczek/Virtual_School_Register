using Microsoft.EntityFrameworkCore.Migrations;

namespace Virtual_School_Register.Data.Migrations
{
    public partial class LessonsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Subject_SubjectId",
                table: "Lesson");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Lesson",
                newName: "ConductingLessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_SubjectId",
                table: "Lesson",
                newName: "IX_Lesson_ConductingLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_ConductingLesson_ConductingLessonId",
                table: "Lesson",
                column: "ConductingLessonId",
                principalTable: "ConductingLesson",
                principalColumn: "ConductingLessonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_ConductingLesson_ConductingLessonId",
                table: "Lesson");

            migrationBuilder.RenameColumn(
                name: "ConductingLessonId",
                table: "Lesson",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_ConductingLessonId",
                table: "Lesson",
                newName: "IX_Lesson_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Subject_SubjectId",
                table: "Lesson",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
