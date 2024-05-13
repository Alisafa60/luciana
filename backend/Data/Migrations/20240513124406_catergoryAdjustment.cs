using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class catergoryAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_ParentCategories_ParentCategotyId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "ParentCategotyId",
                table: "Categories",
                newName: "ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ParentCategotyId",
                table: "Categories",
                newName: "IX_Categories_ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_ParentCategories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId",
                principalTable: "ParentCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_ParentCategories_ParentCategoryId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "ParentCategoryId",
                table: "Categories",
                newName: "ParentCategotyId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                newName: "IX_Categories_ParentCategotyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_ParentCategories_ParentCategotyId",
                table: "Categories",
                column: "ParentCategotyId",
                principalTable: "ParentCategories",
                principalColumn: "Id");
        }
    }
}
