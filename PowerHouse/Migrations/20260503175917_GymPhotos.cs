using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerHouse.Migrations
{
    /// <inheritdoc />
    public partial class GymPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GymPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GymPhotos_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 3, 17, 59, 13, 699, DateTimeKind.Utc).AddTicks(1315));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 3, 17, 59, 13, 699, DateTimeKind.Utc).AddTicks(1462));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 3, 17, 59, 13, 699, DateTimeKind.Utc).AddTicks(1464));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 3, 17, 59, 13, 919, DateTimeKind.Utc).AddTicks(319), "$2a$11$ovoPEWqfpA25kLNlFoVpAuBnhbTi6.cdBF/SDUngkHuatlM1pGNHO" });

            migrationBuilder.CreateIndex(
                name: "IX_GymPhotos_BranchId",
                table: "GymPhotos",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GymPhotos");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 2, 16, 41, 26, 133, DateTimeKind.Utc).AddTicks(1922));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 2, 16, 41, 26, 133, DateTimeKind.Utc).AddTicks(2072));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 2, 16, 41, 26, 133, DateTimeKind.Utc).AddTicks(2073));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 2, 16, 41, 26, 355, DateTimeKind.Utc).AddTicks(1087), "$2a$11$gIzvPGTiO.k.ac2SdBJ48OEvPFAXjnZonY4kI0B3JCk8gjul8Mf9C" });
        }
    }
}
