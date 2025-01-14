using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class new_paymentTransections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "PaymentTransactions",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "PaymentTransactions",
                newName: "ProviderCommissionRateAmount");

            migrationBuilder.AddColumn<string>(
                name: "ConversationId",
                table: "PaymentTransactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FraudStatus",
                table: "PaymentTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Installment",
                table: "PaymentTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidPrice",
                table: "PaymentTransactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "PaymentTransactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "PaymentTransactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProviderCommissionFee",
                table: "PaymentTransactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateOnly>(
                name: "SystemTime",
                table: "PaymentTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "FraudStatus",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "Installment",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "PaidPrice",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "ProviderCommissionFee",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "SystemTime",
                table: "PaymentTransactions");

            migrationBuilder.RenameColumn(
                name: "ProviderCommissionRateAmount",
                table: "PaymentTransactions",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "PaymentTransactions",
                newName: "TransactionId");
        }
    }
}
