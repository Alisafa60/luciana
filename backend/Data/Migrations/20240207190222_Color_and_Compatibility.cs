using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Color_and_Compatibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HexValue = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkinToneColorCompatibilities",
                columns: table => new
                {
                    ColorId = table.Column<int>(type: "integer", nullable: false),
                    SkinToneId = table.Column<int>(type: "integer", nullable: false),
                    CompatibilityScore = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_SkinToneColorCompatibilities_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkinToneColorCompatibilities_SkinTones_SkinToneId",
                        column: x => x.SkinToneId,
                        principalTable: "SkinTones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkinToneColorCompatibilities_ColorId",
                table: "SkinToneColorCompatibilities",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_SkinToneColorCompatibilities_SkinToneId",
                table: "SkinToneColorCompatibilities",
                column: "SkinToneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkinToneColorCompatibilities");

            migrationBuilder.DropTable(
                name: "Colors");
        }
    }
}
