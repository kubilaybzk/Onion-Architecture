using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class orderstatus_addad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isOrdered",
                table: "Orders",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "paidStatus",
                table: "Orders",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isOrdered",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "paidStatus",
                table: "Orders");
        }
    }
}
