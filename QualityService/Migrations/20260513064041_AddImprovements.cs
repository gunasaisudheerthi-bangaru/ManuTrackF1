using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QualityService.Migrations
{
    /// <inheritdoc />
    public partial class AddImprovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Inspections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Defects",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Defects");
        }
    }
}
