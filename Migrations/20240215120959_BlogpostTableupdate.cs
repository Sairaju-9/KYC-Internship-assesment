using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Code_plus.Migrations
{
    /// <inheritdoc />
    public partial class BlogpostTableupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "content",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content",
                table: "BlogPosts");
        }
    }
}
