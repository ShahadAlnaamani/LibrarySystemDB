using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystemDB.Migrations
{
    /// <inheritdoc />
    public partial class CreatingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatName = table.Column<int>(type: "int", nullable: false),
                    NoBooks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CatID);
                });

            migrationBuilder.CreateTable(
                name: "Librarians",
                columns: table => new
                {
                    LID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LUserName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LFName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LLName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Librarians", x => x.LID);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    RID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RLName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    REmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RGender = table.Column<int>(type: "int", nullable: false),
                    RPhoneNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    RUserName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.RID);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BorrowPeriod = table.Column<int>(type: "int", nullable: false),
                    AuthFName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AuthLName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCopies = table.Column<int>(type: "int", nullable: false),
                    BorrowedCopies = table.Column<int>(type: "int", nullable: false),
                    BCID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookID);
                    table.ForeignKey(
                        name: "FK_Books_Categories_BCID",
                        column: x => x.BCID,
                        principalTable: "Categories",
                        principalColumn: "CatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Borrows",
                columns: table => new
                {
                    BBID = table.Column<int>(type: "int", nullable: false),
                    BRID = table.Column<int>(type: "int", nullable: false),
                    BorrowedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PredictedReturn = table.Column<DateOnly>(type: "date", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ActualReturn = table.Column<DateOnly>(type: "date", nullable: false),
                    IsReturned = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => new { x.BBID, x.BRID, x.BorrowedDate });
                    table.ForeignKey(
                        name: "FK_Borrows_Books_BBID",
                        column: x => x.BBID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Borrows_Readers_BRID",
                        column: x => x.BRID,
                        principalTable: "Readers",
                        principalColumn: "RID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BCID",
                table: "Books",
                column: "BCID");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BRID",
                table: "Borrows",
                column: "BRID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Borrows");

            migrationBuilder.DropTable(
                name: "Librarians");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Readers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
