using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderAndPaymentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tab_payment_OrderId",
                table: "tab_payment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_tab_order_UserId",
                table: "tab_order",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tab_order_tab_user_UserId",
                table: "tab_order",
                column: "UserId",
                principalTable: "tab_user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tab_payment_tab_order_OrderId",
                table: "tab_payment",
                column: "OrderId",
                principalTable: "tab_order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tab_order_tab_user_UserId",
                table: "tab_order");

            migrationBuilder.DropForeignKey(
                name: "FK_tab_payment_tab_order_OrderId",
                table: "tab_payment");

            migrationBuilder.DropIndex(
                name: "IX_tab_payment_OrderId",
                table: "tab_payment");

            migrationBuilder.DropIndex(
                name: "IX_tab_order_UserId",
                table: "tab_order");
        }
    }
}
