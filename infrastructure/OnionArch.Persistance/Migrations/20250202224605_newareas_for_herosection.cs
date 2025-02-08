using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class newareas_for_herosection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HtmlContent",
                table: "HeroSectionSliders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSliderImage",
                table: "HeroSectionSliders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HtmlContent",
                table: "HeroSectionSliders");

            migrationBuilder.DropColumn(
                name: "isSliderImage",
                table: "HeroSectionSliders");
        }
    }
}
