using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS2.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsPenaltyPaidInBorrowRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPenaltyPaid",
                table: "BorrowRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPenaltyPaid",
                table: "BorrowRecords");
        }
    }
}
