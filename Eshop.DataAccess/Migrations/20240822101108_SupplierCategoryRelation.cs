using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SupplierCategoryRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Suppliers_SupplierId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_SupplierId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Suppliers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Suppliers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierId",
                table: "Suppliers",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Suppliers_SupplierId",
                table: "Suppliers",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }
    }
}
