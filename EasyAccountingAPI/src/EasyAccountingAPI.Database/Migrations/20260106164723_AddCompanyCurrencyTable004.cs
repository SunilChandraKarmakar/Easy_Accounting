using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyCurrencyTable004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MasterSettings");

            migrationBuilder.EnsureSchema(
                name: "Authentication");

            migrationBuilder.RenameTable(
                name: "UserLoginHistories",
                newName: "UserLoginHistories",
                newSchema: "Authentication");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "Authentication");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employees",
                newSchema: "Authentication");

            migrationBuilder.RenameTable(
                name: "EmployeeRoles",
                newName: "EmployeeRoles",
                newSchema: "Authentication");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Authentication",
                table: "Roles",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                schema: "Authentication",
                table: "Employees",
                type: "nvarchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "MasterSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 50, nullable: false),
                    BaseRate = table.Column<double>(type: "float", nullable: false),
                    Symble = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "MasterSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSellWithPos = table.Column<bool>(type: "bit", nullable: false),
                    IsProductHaveBrand = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Global",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Global",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "MasterSettings",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CityId",
                schema: "MasterSettings",
                table: "Companies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CountryId",
                schema: "MasterSettings",
                table: "Companies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CurrencyId",
                schema: "MasterSettings",
                table: "Companies",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies",
                schema: "MasterSettings");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "MasterSettings");

            migrationBuilder.RenameTable(
                name: "UserLoginHistories",
                schema: "Authentication",
                newName: "UserLoginHistories");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Authentication",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Employees",
                schema: "Authentication",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "EmployeeRoles",
                schema: "Authentication",
                newName: "EmployeeRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldNullable: true);
        }
    }
}
