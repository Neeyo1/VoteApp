using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixOwnerGroupRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                column: "OwnerId",
                unique: true);
        }
    }
}
