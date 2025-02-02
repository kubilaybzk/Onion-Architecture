using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class newarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ProviderCommissionRateAmount",
                table: "PaymentTransactions",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ProviderCommissionFee",
                table: "PaymentTransactions",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "PaymentTransactions",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidPrice",
                table: "PaymentTransactions",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HeroSectionSliderID",
                table: "Files",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSliderImage",
                table: "Files",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalBasketAmount",
                table: "Baskets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "HeroSectionSliders",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageAltTile = table.Column<string>(type: "text", nullable: false),
                    ImageRederictLink = table.Column<string>(type: "text", nullable: false),
                    ImageRedirectLinkTitle = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroSectionSliders", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_HeroSectionSliderID",
                table: "Files",
                column: "HeroSectionSliderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_HeroSectionSliders_HeroSectionSliderID",
                table: "Files",
                column: "HeroSectionSliderID",
                principalTable: "HeroSectionSliders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_HeroSectionSliders_HeroSectionSliderID",
                table: "Files");

            migrationBuilder.DropTable(
                name: "HeroSectionSliders");

            migrationBuilder.DropIndex(
                name: "IX_Files_HeroSectionSliderID",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "HeroSectionSliderID",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "isSliderImage",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "TotalBasketAmount",
                table: "Baskets");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProviderCommissionRateAmount",
                table: "PaymentTransactions",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ProviderCommissionFee",
                table: "PaymentTransactions",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "PaymentTransactions",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidPrice",
                table: "PaymentTransactions",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }
    }
}
