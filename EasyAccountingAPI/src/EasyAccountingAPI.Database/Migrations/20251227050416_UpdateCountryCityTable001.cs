using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCountryCityTable001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Countries_Name_Code",
                schema: "Global",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Name",
                schema: "Global",
                table: "Cities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name_Code",
                schema: "Global",
                table: "Countries",
                columns: new[] { "Name", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name",
                schema: "Global",
                table: "Cities",
                column: "Name",
                unique: true);
        }
    }
}
