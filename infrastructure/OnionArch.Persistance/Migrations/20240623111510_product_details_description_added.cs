using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class product_details_description_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "SmallDescription");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SmallDescription",
                table: "Products",
                newName: "Description");
        }
    }
}
