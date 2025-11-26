using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class ComplaintVersioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintHistories_Complaints_ComplaintId",
                table: "ComplaintHistories");

            migrationBuilder.DropIndex(
                name: "IX_ComplaintHistories_ComplaintId",
                table: "ComplaintHistories");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Complaints",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "ComplaintHistories",
                newName: "Title");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "ComplaintHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ComplaintHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ComplaintHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ComplaintHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintHistories_DepartmentId",
                table: "ComplaintHistories",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintHistories_Departments_DepartmentId",
                table: "ComplaintHistories",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintHistories_Departments_DepartmentId",
                table: "ComplaintHistories");

            migrationBuilder.DropIndex(
                name: "IX_ComplaintHistories_DepartmentId",
                table: "ComplaintHistories");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ComplaintHistories");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ComplaintHistories");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ComplaintHistories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ComplaintHistories");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Complaints",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ComplaintHistories",
                newName: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintHistories_ComplaintId",
                table: "ComplaintHistories",
                column: "ComplaintId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintHistories_Complaints_ComplaintId",
                table: "ComplaintHistories",
                column: "ComplaintId",
                principalTable: "Complaints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
