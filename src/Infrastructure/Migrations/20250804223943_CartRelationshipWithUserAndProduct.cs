using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CartRelationshipWithUserAndProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tab_cart_ProductId",
                table: "tab_cart",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_tab_cart_UserId",
                table: "tab_cart",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tab_cart_tab_products_ProductId",
                table: "tab_cart",
                column: "ProductId",
                principalTable: "tab_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tab_cart_tab_user_UserId",
                table: "tab_cart",
                column: "UserId",
                principalTable: "tab_user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tab_cart_tab_products_ProductId",
                table: "tab_cart");

            migrationBuilder.DropForeignKey(
                name: "FK_tab_cart_tab_user_UserId",
                table: "tab_cart");

            migrationBuilder.DropIndex(
                name: "IX_tab_cart_ProductId",
                table: "tab_cart");

            migrationBuilder.DropIndex(
                name: "IX_tab_cart_UserId",
                table: "tab_cart");
        }
    }
}
