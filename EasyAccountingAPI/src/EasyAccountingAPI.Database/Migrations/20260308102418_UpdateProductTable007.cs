using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductTable007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "ProductService",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId",
                schema: "ProductService",
                table: "Products",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Companies_CompanyId",
                schema: "ProductService",
                table: "Products",
                column: "CompanyId",
                principalSchema: "MasterSettings",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Companies_CompanyId",
                schema: "ProductService",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CompanyId",
                schema: "ProductService",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "ProductService",
                table: "Products");
        }
    }
}
