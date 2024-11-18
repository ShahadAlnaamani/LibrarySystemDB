using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystemDB.Migrations
{
    /// <inheritdoc />
    public partial class CategoryName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryTypes",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryTypes",
                table: "Categories");
        }
    }
}
