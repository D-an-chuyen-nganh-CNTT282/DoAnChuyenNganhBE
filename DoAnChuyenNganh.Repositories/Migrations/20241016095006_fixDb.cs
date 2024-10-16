using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnChuyenNganh.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class fixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipManagement_Business_BusinessId1",
                table: "InternshipManagement");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipManagement_Student_StudentId1",
                table: "InternshipManagement");

            migrationBuilder.DropIndex(
                name: "IX_InternshipManagement_BusinessId1",
                table: "InternshipManagement");

            migrationBuilder.DropIndex(
                name: "IX_InternshipManagement_StudentId1",
                table: "InternshipManagement");

            migrationBuilder.DropColumn(
                name: "BusinessId1",
                table: "InternshipManagement");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "InternshipManagement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessId1",
                table: "InternshipManagement",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId1",
                table: "InternshipManagement",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipManagement_BusinessId1",
                table: "InternshipManagement",
                column: "BusinessId1");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipManagement_StudentId1",
                table: "InternshipManagement",
                column: "StudentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipManagement_Business_BusinessId1",
                table: "InternshipManagement",
                column: "BusinessId1",
                principalTable: "Business",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipManagement_Student_StudentId1",
                table: "InternshipManagement",
                column: "StudentId1",
                principalTable: "Student",
                principalColumn: "Id");
        }
    }
}
