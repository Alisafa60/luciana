using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SkinTone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkinToneId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SkinTone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HexValue = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkinTone", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SkinToneId",
                table: "Users",
                column: "SkinToneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SkinTone_SkinToneId",
                table: "Users",
                column: "SkinToneId",
                principalTable: "SkinTone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SkinTone_SkinToneId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "SkinTone");

            migrationBuilder.DropIndex(
                name: "IX_Users_SkinToneId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SkinToneId",
                table: "Users");
        }
    }
}
