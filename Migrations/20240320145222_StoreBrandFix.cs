using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryWorld.Migrations
{
    /// <inheritdoc />
    public partial class StoreBrandFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Stores_StoresName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandsName",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "StoresName",
                table: "Products",
                newName: "StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_StoresName",
                table: "Products",
                newName: "IX_Products_StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Stores_StoreId",
                table: "Products",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Stores_StoreId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "Products",
                newName: "StoresName");

            migrationBuilder.RenameIndex(
                name: "IX_Products_StoreId",
                table: "Products",
                newName: "IX_Products_StoresName");

            migrationBuilder.AddColumn<int>(
                name: "BrandsName",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Stores_StoresName",
                table: "Products",
                column: "StoresName",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
