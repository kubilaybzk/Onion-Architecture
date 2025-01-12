using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Orders_OrderID",
                table: "PaymentTransactions");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "PaymentTransactions",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_OrderID",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Orders_OrderId",
                table: "PaymentTransactions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Orders_OrderId",
                table: "PaymentTransactions");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "PaymentTransactions",
                newName: "OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_OrderId",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Orders_OrderID",
                table: "PaymentTransactions",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
