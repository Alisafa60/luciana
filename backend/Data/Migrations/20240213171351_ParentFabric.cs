using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ParentFabric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_ParentColor_ParentColorId",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Fabrics_Fabrics_ParentFabricId",
                table: "Fabrics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentColor",
                table: "ParentColor");

            migrationBuilder.RenameTable(
                name: "ParentColor",
                newName: "ParentColors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentColors",
                table: "ParentColors",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ParentFabrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentFabrics", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_ParentColors_ParentColorId",
                table: "Colors",
                column: "ParentColorId",
                principalTable: "ParentColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fabrics_ParentFabrics_ParentFabricId",
                table: "Fabrics",
                column: "ParentFabricId",
                principalTable: "ParentFabrics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_ParentColors_ParentColorId",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Fabrics_ParentFabrics_ParentFabricId",
                table: "Fabrics");

            migrationBuilder.DropTable(
                name: "ParentFabrics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentColors",
                table: "ParentColors");

            migrationBuilder.RenameTable(
                name: "ParentColors",
                newName: "ParentColor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentColor",
                table: "ParentColor",
                column: "Id");

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
        }
    }
}
