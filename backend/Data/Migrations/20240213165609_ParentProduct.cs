using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ParentProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_ParentCategotyId",
                table: "Category");

            migrationBuilder.CreateTable(
                name: "ParentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentCategories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Category_ParentCategories_ParentCategotyId",
                table: "Category",
                column: "ParentCategotyId",
                principalTable: "ParentCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_ParentCategories_ParentCategotyId",
                table: "Category");

            migrationBuilder.DropTable(
                name: "ParentCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_ParentCategotyId",
                table: "Category",
                column: "ParentCategotyId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}
