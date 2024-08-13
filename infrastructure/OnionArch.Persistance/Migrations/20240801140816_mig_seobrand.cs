using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_seobrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BrandSmallDescription",
                table: "Brands",
                newName: "SeoLinkTitle");

            migrationBuilder.RenameColumn(
                name: "BrandLongDescription",
                table: "Brands",
                newName: "SeoLinkDescription");

            migrationBuilder.RenameColumn(
                name: "BrandIsActive",
                table: "Brands",
                newName: "isActive");

            migrationBuilder.AddColumn<string>(
                name: "SeoImageAltInformation",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandSlug",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DetailDescription",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DetailTitle",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDetailDescription",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDetailTitle",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalProductCount",
                table: "Brands",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeoImageAltInformation",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "BrandSlug",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DetailDescription",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DetailTitle",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "SeoDetailDescription",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "SeoDetailTitle",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "TotalProductCount",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Brands",
                newName: "BrandIsActive");

            migrationBuilder.RenameColumn(
                name: "SeoLinkTitle",
                table: "Brands",
                newName: "BrandSmallDescription");

            migrationBuilder.RenameColumn(
                name: "SeoLinkDescription",
                table: "Brands",
                newName: "BrandLongDescription");
        }
    }
}
