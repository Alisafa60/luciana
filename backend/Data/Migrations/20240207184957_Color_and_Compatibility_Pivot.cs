using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Color_and_Compatibility_Pivot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SkinTone_SkinToneId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkinTone",
                table: "SkinTone");

            migrationBuilder.RenameTable(
                name: "SkinTone",
                newName: "SkinTones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkinTones",
                table: "SkinTones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SkinTones_SkinToneId",
                table: "Users",
                column: "SkinToneId",
                principalTable: "SkinTones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SkinTones_SkinToneId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkinTones",
                table: "SkinTones");

            migrationBuilder.RenameTable(
                name: "SkinTones",
                newName: "SkinTone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkinTone",
                table: "SkinTone",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SkinTone_SkinToneId",
                table: "Users",
                column: "SkinToneId",
                principalTable: "SkinTone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
