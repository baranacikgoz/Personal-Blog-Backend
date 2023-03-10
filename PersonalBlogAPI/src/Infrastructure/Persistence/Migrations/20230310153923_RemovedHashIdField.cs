using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedHashIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "HashId",
                table: "ArticleTags");

            migrationBuilder.DropColumn(
                name: "HashId",
                table: "Articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashId",
                table: "Tags",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HashId",
                table: "ArticleTags",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HashId",
                table: "Articles",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true);
        }
    }
}
