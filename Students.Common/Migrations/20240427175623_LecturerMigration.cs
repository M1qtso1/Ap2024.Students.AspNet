using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Students.Common.Migrations
{
    /// <inheritdoc />
    public partial class LecturerMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LecturerId",
                table: "Subject",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lecturer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subject_LecturerId",
                table: "Subject",
                column: "LecturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Lecturer_LecturerId",
                table: "Subject",
                column: "LecturerId",
                principalTable: "Lecturer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Lecturer_LecturerId",
                table: "Subject");

            migrationBuilder.DropTable(
                name: "Lecturer");

            migrationBuilder.DropIndex(
                name: "IX_Subject_LecturerId",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "LecturerId",
                table: "Subject");
        }
    }
}
