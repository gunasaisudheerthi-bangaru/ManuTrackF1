using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QualityService.Migrations
{
    /// <inheritdoc />
    public partial class RenameDefectResolutionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resolution",
                table: "Defects",
                newName: "ResolutionDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResolutionDescription",
                table: "Defects",
                newName: "Resolution");
        }
    }
}
