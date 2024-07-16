using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_2414 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributes_Attributes_AttributeId",
                table: "CategoryAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributes_Categories_CategoryId",
                table: "CategoryAttributes");

            migrationBuilder.DropIndex(
                name: "IX_CategoryAttributes_AttributeId",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "AttributeId",
                table: "CategoryAttributes");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "CategoryAttributes",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryAttributes_CategoryId",
                table: "CategoryAttributes",
                newName: "IX_CategoryAttributes_CategoryID");

            migrationBuilder.CreateTable(
                name: "AttributeCategoryAttribute",
                columns: table => new
                {
                    CategoryAttributesID = table.Column<Guid>(type: "uuid", nullable: false),
                    FiltersID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeCategoryAttribute", x => new { x.CategoryAttributesID, x.FiltersID });
                    table.ForeignKey(
                        name: "FK_AttributeCategoryAttribute_Attributes_FiltersID",
                        column: x => x.FiltersID,
                        principalTable: "Attributes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttributeCategoryAttribute_CategoryAttributes_CategoryAttri~",
                        column: x => x.CategoryAttributesID,
                        principalTable: "CategoryAttributes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeCategoryAttribute_FiltersID",
                table: "AttributeCategoryAttribute",
                column: "FiltersID");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributes_Categories_CategoryID",
                table: "CategoryAttributes",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributes_Categories_CategoryID",
                table: "CategoryAttributes");

            migrationBuilder.DropTable(
                name: "AttributeCategoryAttribute");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "CategoryAttributes",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryAttributes_CategoryID",
                table: "CategoryAttributes",
                newName: "IX_CategoryAttributes_CategoryId");

            migrationBuilder.AddColumn<Guid>(
                name: "AttributeId",
                table: "CategoryAttributes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAttributes_AttributeId",
                table: "CategoryAttributes",
                column: "AttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributes_Attributes_AttributeId",
                table: "CategoryAttributes",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributes_Categories_CategoryId",
                table: "CategoryAttributes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
