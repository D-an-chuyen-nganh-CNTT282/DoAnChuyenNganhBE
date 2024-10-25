using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnChuyenNganh.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnFileScanUrlOutgoingDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileScanUrl",
                table: "OutgoingDocument",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileScanUrl",
                table: "OutgoingDocument");
        }
    }
}
