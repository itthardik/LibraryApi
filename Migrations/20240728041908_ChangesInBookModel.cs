using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS2.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Publiser_Name",
                table: "books",
                newName: "PubliserName");

            migrationBuilder.RenameColumn(
                name: "Publiser_Description",
                table: "books",
                newName: "PubliserDescription");

            migrationBuilder.RenameColumn(
                name: "Current_Stock",
                table: "books",
                newName: "CurrentStock");

            migrationBuilder.RenameColumn(
                name: "Author_Name",
                table: "books",
                newName: "AuthorName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PubliserName",
                table: "books",
                newName: "Publiser_Name");

            migrationBuilder.RenameColumn(
                name: "PubliserDescription",
                table: "books",
                newName: "Publiser_Description");

            migrationBuilder.RenameColumn(
                name: "CurrentStock",
                table: "books",
                newName: "Current_Stock");

            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "books",
                newName: "Author_Name");
        }
    }
}
