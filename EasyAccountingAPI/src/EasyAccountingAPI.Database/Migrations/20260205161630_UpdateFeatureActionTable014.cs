using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeatureActionTable014 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureActions",
                schema: "MasterSettings",
                table: "FeatureActions");

            // 2. Drop old string Id column
            migrationBuilder.DropColumn(
                name: "Id",
                schema: "MasterSettings",
                table: "FeatureActions");

            // 3. Add new INT IDENTITY Id column
            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "MasterSettings",
                table: "FeatureActions",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // 4. Recreate primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureActions",
                schema: "MasterSettings",
                table: "FeatureActions",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureActions",
                schema: "MasterSettings",
                table: "FeatureActions");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "MasterSettings",
                table: "FeatureActions");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                schema: "MasterSettings",
                table: "FeatureActions",
                type: "nvarchar(450)",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureActions",
                schema: "MasterSettings",
                table: "FeatureActions",
                column: "Id");
        }
    }
}
