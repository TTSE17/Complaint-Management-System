using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaintHistoryInComplaintEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintHistories_AspNetUsers_UserId",
                table: "ComplaintHistories");

            migrationBuilder.DropIndex(
                name: "IX_ComplaintHistories_UserId",
                table: "ComplaintHistories");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintHistories_Complaints_ComplaintId",
                table: "ComplaintHistories");

            migrationBuilder.DropIndex(
                name: "IX_ComplaintHistories_ComplaintId",
                table: "ComplaintHistories");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintHistories_UserId",
                table: "ComplaintHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintHistories_AspNetUsers_UserId",
                table: "ComplaintHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
