using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ParentColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_ParentCategories_ParentCategotyId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Fabric_Fabric_ParentFabricId",
                table: "Fabric");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Category_CategoryId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFabrics_Fabric_FabricId",
                table: "ProductFabrics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fabric",
                table: "Fabric");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Fabric",
                newName: "Fabrics");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Fabric_ParentFabricId",
                table: "Fabrics",
                newName: "IX_Fabrics_ParentFabricId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_ParentCategotyId",
                table: "Categories",
                newName: "IX_Categories_ParentCategotyId");

            migrationBuilder.AddColumn<int>(
                name: "ParentColorId",
                table: "Colors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fabrics",
                table: "Fabrics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ParentColor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentColor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ParentColorId",
                table: "Colors",
                column: "ParentColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_ParentCategories_ParentCategotyId",
                table: "Categories",
                column: "ParentCategotyId",
                principalTable: "ParentCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_ParentColor_ParentColorId",
                table: "Colors",
                column: "ParentColorId",
                principalTable: "ParentColor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fabrics_Fabrics_ParentFabricId",
                table: "Fabrics",
                column: "ParentFabricId",
                principalTable: "Fabrics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFabrics_Fabrics_FabricId",
                table: "ProductFabrics",
                column: "FabricId",
                principalTable: "Fabrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_ParentCategories_ParentCategotyId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Colors_ParentColor_ParentColorId",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Fabrics_Fabrics_ParentFabricId",
                table: "Fabrics");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFabrics_Fabrics_FabricId",
                table: "ProductFabrics");

            migrationBuilder.DropTable(
                name: "ParentColor");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ParentColorId",
                table: "Colors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fabrics",
                table: "Fabrics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ParentColorId",
                table: "Colors");

            migrationBuilder.RenameTable(
                name: "Fabrics",
                newName: "Fabric");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Fabrics_ParentFabricId",
                table: "Fabric",
                newName: "IX_Fabric_ParentFabricId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ParentCategotyId",
                table: "Category",
                newName: "IX_Category_ParentCategotyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fabric",
                table: "Fabric",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_ParentCategories_ParentCategotyId",
                table: "Category",
                column: "ParentCategotyId",
                principalTable: "ParentCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fabric_Fabric_ParentFabricId",
                table: "Fabric",
                column: "ParentFabricId",
                principalTable: "Fabric",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Category_CategoryId",
                table: "ProductCategories",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFabrics_Fabric_FabricId",
                table: "ProductFabrics",
                column: "FabricId",
                principalTable: "Fabric",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
