using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hau_backend.Migrations
{
    /// <inheritdoc />
    public partial class SwitchToAzureBlobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Pictures",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "News",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "News");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Pictures",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "News",
                type: "bytea",
                nullable: true);
        }
    }
}
