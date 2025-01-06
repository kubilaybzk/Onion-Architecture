using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class all_new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DiscountCouponId",
                table: "Baskets",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountedAmount",
                table: "Baskets",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountCoupons",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MinimumCartAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    IsPercentage = table.Column<bool>(type: "boolean", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsPersonal = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    MaxUsageCount = table.Column<int>(type: "integer", nullable: false),
                    UsedCount = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCoupons", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DiscountCoupons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_DiscountCouponId",
                table: "Baskets",
                column: "DiscountCouponId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCoupons_Code",
                table: "DiscountCoupons",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCoupons_UserId",
                table: "DiscountCoupons",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_DiscountCoupons_DiscountCouponId",
                table: "Baskets",
                column: "DiscountCouponId",
                principalTable: "DiscountCoupons",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_DiscountCoupons_DiscountCouponId",
                table: "Baskets");

            migrationBuilder.DropTable(
                name: "DiscountCoupons");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_DiscountCouponId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "DiscountCouponId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "DiscountedAmount",
                table: "Baskets");
        }
    }
}
