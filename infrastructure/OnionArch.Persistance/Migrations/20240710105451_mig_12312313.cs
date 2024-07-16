using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_12312313 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CategoryAttributes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "CategoryAttributes");
        }
    }
}
