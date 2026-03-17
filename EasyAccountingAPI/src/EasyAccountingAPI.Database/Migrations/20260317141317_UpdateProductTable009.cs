using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductTable009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorAddresses_Vendors_VendorId",
                schema: "MasterSettings",
                table: "VendorAddresses");

            migrationBuilder.AddColumn<int>(
                name: "TotalQuantity",
                schema: "ProductService",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorAddresses_Vendors_VendorId",
                schema: "MasterSettings",
                table: "VendorAddresses",
                column: "VendorId",
                principalSchema: "MasterSettings",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorAddresses_Vendors_VendorId",
                schema: "MasterSettings",
                table: "VendorAddresses");

            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                schema: "ProductService",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorAddresses_Vendors_VendorId",
                schema: "MasterSettings",
                table: "VendorAddresses",
                column: "VendorId",
                principalSchema: "MasterSettings",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
