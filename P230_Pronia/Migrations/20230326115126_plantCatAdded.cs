using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P230_Pronia.Migrations
{
    public partial class plantCatAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantCategory_Categories_CategoryId",
                table: "PlantCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantCategory_Plants_PlantId",
                table: "PlantCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantCategory",
                table: "PlantCategory");

            migrationBuilder.RenameTable(
                name: "PlantCategory",
                newName: "PlantCategories");

            migrationBuilder.RenameIndex(
                name: "IX_PlantCategory_PlantId",
                table: "PlantCategories",
                newName: "IX_PlantCategories_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantCategory_CategoryId",
                table: "PlantCategories",
                newName: "IX_PlantCategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantCategories",
                table: "PlantCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantCategories_Categories_CategoryId",
                table: "PlantCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantCategories_Plants_PlantId",
                table: "PlantCategories",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantCategories_Categories_CategoryId",
                table: "PlantCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantCategories_Plants_PlantId",
                table: "PlantCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantCategories",
                table: "PlantCategories");

            migrationBuilder.RenameTable(
                name: "PlantCategories",
                newName: "PlantCategory");

            migrationBuilder.RenameIndex(
                name: "IX_PlantCategories_PlantId",
                table: "PlantCategory",
                newName: "IX_PlantCategory_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantCategories_CategoryId",
                table: "PlantCategory",
                newName: "IX_PlantCategory_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantCategory",
                table: "PlantCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantCategory_Categories_CategoryId",
                table: "PlantCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantCategory_Plants_PlantId",
                table: "PlantCategory",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
