using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ProductFabric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTexturePatterns_TexturePattern_TexturePatternId",
                table: "ProductTexturePatterns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TexturePattern",
                table: "TexturePattern");

            migrationBuilder.RenameTable(
                name: "TexturePattern",
                newName: "TexturePatterns");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TexturePatterns",
                table: "TexturePatterns",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Fabric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentFabricId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabric", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fabric_Fabric_ParentFabricId",
                        column: x => x.ParentFabricId,
                        principalTable: "Fabric",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductFabrics",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    FabricId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFabrics", x => new { x.ProductId, x.FabricId });
                    table.ForeignKey(
                        name: "FK_ProductFabrics_Fabric_FabricId",
                        column: x => x.FabricId,
                        principalTable: "Fabric",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductFabrics_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fabric_ParentFabricId",
                table: "Fabric",
                column: "ParentFabricId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFabrics_FabricId",
                table: "ProductFabrics",
                column: "FabricId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTexturePatterns_TexturePatterns_TexturePatternId",
                table: "ProductTexturePatterns",
                column: "TexturePatternId",
                principalTable: "TexturePatterns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTexturePatterns_TexturePatterns_TexturePatternId",
                table: "ProductTexturePatterns");

            migrationBuilder.DropTable(
                name: "ProductFabrics");

            migrationBuilder.DropTable(
                name: "Fabric");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TexturePatterns",
                table: "TexturePatterns");

            migrationBuilder.RenameTable(
                name: "TexturePatterns",
                newName: "TexturePattern");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TexturePattern",
                table: "TexturePattern",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTexturePatterns_TexturePattern_TexturePatternId",
                table: "ProductTexturePatterns",
                column: "TexturePatternId",
                principalTable: "TexturePattern",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
