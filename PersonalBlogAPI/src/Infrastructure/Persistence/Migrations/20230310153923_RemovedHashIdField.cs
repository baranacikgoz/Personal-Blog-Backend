using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

# pragma warning disable IDE0161
namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedHashIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "HashId",
                table: "Tags");

            _ = migrationBuilder.DropColumn(
                name: "HashId",
                table: "ArticleTags");

            _ = migrationBuilder.DropColumn(
                name: "HashId",
                table: "Articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "HashId",
                table: "Tags",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "HashId",
                table: "ArticleTags",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "HashId",
                table: "Articles",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true);
        }
    }
}
