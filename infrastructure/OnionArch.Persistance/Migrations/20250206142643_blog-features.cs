using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class blogfeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BlogID",
                table: "Files",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHeader",
                table: "Files",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReadingTime = table.Column<int>(type: "integer", nullable: false),
                    SeoTitle = table.Column<string>(type: "text", nullable: false),
                    SeoDescription = table.Column<string>(type: "text", nullable: false),
                    SeoKeywords = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_BlogID",
                table: "Files",
                column: "BlogID");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Blogs_BlogID",
                table: "Files",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Blogs_BlogID",
                table: "Files");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Files_BlogID",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AltText",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "BlogID",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "IsHeader",
                table: "Files");
        }
    }
}
