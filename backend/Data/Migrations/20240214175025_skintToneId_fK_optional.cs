using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class skintToneId_fK_optional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Size_ProductSizeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SkinTones_SkinToneId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Size",
                table: "Size");

            migrationBuilder.RenameTable(
                name: "Size",
                newName: "Sizes");

            migrationBuilder.AlterColumn<int>(
                name: "SkinToneId",
                table: "Users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Sizes_ProductSizeId",
                table: "Products",
                column: "ProductSizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SkinTones_SkinToneId",
                table: "Users",
                column: "SkinToneId",
                principalTable: "SkinTones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Sizes_ProductSizeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SkinTones_SkinToneId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes");

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "Size");

            migrationBuilder.AlterColumn<int>(
                name: "SkinToneId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Size",
                table: "Size",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Size_ProductSizeId",
                table: "Products",
                column: "ProductSizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SkinTones_SkinToneId",
                table: "Users",
                column: "SkinToneId",
                principalTable: "SkinTones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
