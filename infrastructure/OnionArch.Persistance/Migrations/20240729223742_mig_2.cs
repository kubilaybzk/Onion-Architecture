using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Files_ID",
                table: "Brands");

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Files",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_BrandId",
                table: "Files",
                column: "BrandId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Brands_BrandId",
                table: "Files",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Brands_BrandId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_BrandId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Files");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Files_ID",
                table: "Brands",
                column: "ID",
                principalTable: "Files",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
