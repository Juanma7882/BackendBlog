using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogPersonal.Migrations
{
    /// <inheritdoc />
    public partial class secondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "TbBlog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Enlace",
                table: "TbBlog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Etiquetas",
                table: "TbBlog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "TbBlog");

            migrationBuilder.DropColumn(
                name: "Enlace",
                table: "TbBlog");

            migrationBuilder.DropColumn(
                name: "Etiquetas",
                table: "TbBlog");
        }
    }
}
