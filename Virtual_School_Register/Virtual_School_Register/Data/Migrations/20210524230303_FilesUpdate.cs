using Microsoft.EntityFrameworkCore.Migrations;

namespace Virtual_School_Register.Data.Migrations
{
    public partial class FilesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Lesson_LessonId",
                table: "File");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "File",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_File_LessonId",
                table: "File",
                newName: "IX_File_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Subject_SubjectId",
                table: "File",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Subject_SubjectId",
                table: "File");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "File",
                newName: "LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_File_SubjectId",
                table: "File",
                newName: "IX_File_LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Lesson_LessonId",
                table: "File",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
