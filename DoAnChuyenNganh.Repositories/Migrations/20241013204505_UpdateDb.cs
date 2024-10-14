using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnChuyenNganh.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "OutgoingDocument",
                newName: "OutgoingDocumentTitle");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "OutgoingDocument",
                newName: "OutgoingDocumentContent");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "LecturerPlan",
                newName: "PlanContent");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "IncomingDocument",
                newName: "IncomingDocumentTitle");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "IncomingDocument",
                newName: "IncomingDocumentContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OutgoingDocumentTitle",
                table: "OutgoingDocument",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "OutgoingDocumentContent",
                table: "OutgoingDocument",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "PlanContent",
                table: "LecturerPlan",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "IncomingDocumentTitle",
                table: "IncomingDocument",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "IncomingDocumentContent",
                table: "IncomingDocument",
                newName: "Content");
        }
    }
}
