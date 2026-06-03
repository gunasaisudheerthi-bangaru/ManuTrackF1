using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderService.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "WorkOrderTasks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WorkOrderTasks");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "WorkOrderTasks");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "WorkOrderTasks");

            migrationBuilder.DropColumn(
                name: "ActualEndDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ActualStartDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "AssignedOperatorID",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "EstimatedEndDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "EstimatedStartDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "WorkOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "WorkOrderTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WorkOrderTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "WorkOrderTasks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "WorkOrderTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualEndDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualStartDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedOperatorID",
                table: "WorkOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "WorkOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedEndDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedStartDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "WorkOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
