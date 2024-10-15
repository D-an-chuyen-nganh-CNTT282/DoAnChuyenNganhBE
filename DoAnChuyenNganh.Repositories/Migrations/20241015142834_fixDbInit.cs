using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnChuyenNganh.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class fixDbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumniActivities_Alumni_AlumniId",
                table: "AlumniActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessActivities_Business_BusinessId",
                table: "BusinessActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtracurricularActivities_Student_StudentId",
                table: "ExtracurricularActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_LecturerActivities_Lecturer_GiangVienId",
                table: "LecturerActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LecturerActivities",
                table: "LecturerActivities");

            migrationBuilder.DropIndex(
                name: "IX_LecturerActivities_GiangVienId",
                table: "LecturerActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtracurricularActivities",
                table: "ExtracurricularActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessActivities",
                table: "BusinessActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlumniCompany",
                table: "AlumniCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlumniActivities",
                table: "AlumniActivities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LecturerActivities");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "LecturerActivities");

            migrationBuilder.DropColumn(
                name: "GiangVienId",
                table: "LecturerActivities");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "LecturerActivities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LecturerActivities");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "ExtracurricularActivities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExtracurricularActivities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BusinessActivities");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "BusinessActivities");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "BusinessActivities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BusinessActivities");

            migrationBuilder.DropColumn(
                name: "ActivityName",
                table: "AlumniActivities");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "AlumniActivities");

            migrationBuilder.AlterColumn<string>(
                name: "LecturerId",
                table: "LecturerActivities",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ActivitiesId",
                table: "LecturerActivities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ActivitiesId",
                table: "ExtracurricularActivities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ActivitiesId",
                table: "BusinessActivities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AlumniCompany",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AlumniCompany",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Duty",
                table: "AlumniCompany",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "AlumniCompany",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "AlumniId",
                table: "AlumniCompany",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<string>(
                name: "ActivitiesId",
                table: "AlumniActivities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LecturerActivities",
                table: "LecturerActivities",
                columns: new[] { "Id", "LecturerId", "ActivitiesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtracurricularActivities",
                table: "ExtracurricularActivities",
                columns: new[] { "Id", "StudentId", "ActivitiesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessActivities",
                table: "BusinessActivities",
                columns: new[] { "Id", "BusinessId", "ActivitiesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlumniCompany",
                table: "AlumniCompany",
                columns: new[] { "Id", "AlumniId", "CompanyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlumniActivities",
                table: "AlumniActivities",
                columns: new[] { "Id", "AlumniId", "ActivitiesId" });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventTypes = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LecturerActivities_ActivitiesId",
                table: "LecturerActivities",
                column: "ActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerActivities_LecturerId",
                table: "LecturerActivities",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtracurricularActivities_ActivitiesId",
                table: "ExtracurricularActivities",
                column: "ActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessActivities_ActivitiesId",
                table: "BusinessActivities",
                column: "ActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumniCompany_AlumniId",
                table: "AlumniCompany",
                column: "AlumniId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumniActivities_ActivitiesId",
                table: "AlumniActivities",
                column: "ActivitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlumniActivities_Activities_ActivitiesId",
                table: "AlumniActivities",
                column: "ActivitiesId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlumniActivities_Alumni_AlumniId",
                table: "AlumniActivities",
                column: "AlumniId",
                principalTable: "Alumni",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessActivities_Activities_ActivitiesId",
                table: "BusinessActivities",
                column: "ActivitiesId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessActivities_Business_BusinessId",
                table: "BusinessActivities",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtracurricularActivities_Activities_ActivitiesId",
                table: "ExtracurricularActivities",
                column: "ActivitiesId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtracurricularActivities_Student_StudentId",
                table: "ExtracurricularActivities",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LecturerActivities_Activities_ActivitiesId",
                table: "LecturerActivities",
                column: "ActivitiesId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LecturerActivities_Lecturer_LecturerId",
                table: "LecturerActivities",
                column: "LecturerId",
                principalTable: "Lecturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumniActivities_Activities_ActivitiesId",
                table: "AlumniActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_AlumniActivities_Alumni_AlumniId",
                table: "AlumniActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessActivities_Activities_ActivitiesId",
                table: "BusinessActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessActivities_Business_BusinessId",
                table: "BusinessActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtracurricularActivities_Activities_ActivitiesId",
                table: "ExtracurricularActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtracurricularActivities_Student_StudentId",
                table: "ExtracurricularActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_LecturerActivities_Activities_ActivitiesId",
                table: "LecturerActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_LecturerActivities_Lecturer_LecturerId",
                table: "LecturerActivities");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LecturerActivities",
                table: "LecturerActivities");

            migrationBuilder.DropIndex(
                name: "IX_LecturerActivities_ActivitiesId",
                table: "LecturerActivities");

            migrationBuilder.DropIndex(
                name: "IX_LecturerActivities_LecturerId",
                table: "LecturerActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtracurricularActivities",
                table: "ExtracurricularActivities");

            migrationBuilder.DropIndex(
                name: "IX_ExtracurricularActivities_ActivitiesId",
                table: "ExtracurricularActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessActivities",
                table: "BusinessActivities");

            migrationBuilder.DropIndex(
                name: "IX_BusinessActivities_ActivitiesId",
                table: "BusinessActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlumniCompany",
                table: "AlumniCompany");

            migrationBuilder.DropIndex(
                name: "IX_AlumniCompany_AlumniId",
                table: "AlumniCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlumniActivities",
                table: "AlumniActivities");

            migrationBuilder.DropIndex(
                name: "IX_AlumniActivities_ActivitiesId",
                table: "AlumniActivities");

            migrationBuilder.DropColumn(
                name: "ActivitiesId",
                table: "LecturerActivities");

            migrationBuilder.DropColumn(
                name: "ActivitiesId",
                table: "ExtracurricularActivities");

            migrationBuilder.DropColumn(
                name: "ActivitiesId",
                table: "BusinessActivities");

            migrationBuilder.DropColumn(
                name: "ActivitiesId",
                table: "AlumniActivities");

            migrationBuilder.AlterColumn<string>(
                name: "LecturerId",
                table: "LecturerActivities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LecturerActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "LecturerActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GiangVienId",
                table: "LecturerActivities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "LecturerActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LecturerActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "ExtracurricularActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ExtracurricularActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BusinessActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "BusinessActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "BusinessActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BusinessActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AlumniCompany",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Duty",
                table: "AlumniCompany",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "AlumniCompany",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "AlumniId",
                table: "AlumniCompany",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AlumniCompany",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ActivityName",
                table: "AlumniActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "AlumniActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LecturerActivities",
                table: "LecturerActivities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtracurricularActivities",
                table: "ExtracurricularActivities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessActivities",
                table: "BusinessActivities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlumniCompany",
                table: "AlumniCompany",
                columns: new[] { "AlumniId", "CompanyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlumniActivities",
                table: "AlumniActivities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerActivities_GiangVienId",
                table: "LecturerActivities",
                column: "GiangVienId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlumniActivities_Alumni_AlumniId",
                table: "AlumniActivities",
                column: "AlumniId",
                principalTable: "Alumni",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessActivities_Business_BusinessId",
                table: "BusinessActivities",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtracurricularActivities_Student_StudentId",
                table: "ExtracurricularActivities",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LecturerActivities_Lecturer_GiangVienId",
                table: "LecturerActivities",
                column: "GiangVienId",
                principalTable: "Lecturer",
                principalColumn: "Id");
        }
    }
}
