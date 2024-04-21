using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsPro.Migrations
{
    /// <inheritdoc />
    public partial class updatedRegistrationModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Registrations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1002, 1 },
                column: "RegistrationDate",
                value: new DateTime(2024, 4, 11, 2, 35, 17, 473, DateTimeKind.Local).AddTicks(5372));

            migrationBuilder.UpdateData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1002, 3 },
                column: "RegistrationDate",
                value: new DateTime(2024, 4, 11, 2, 35, 17, 473, DateTimeKind.Local).AddTicks(5376));

            migrationBuilder.UpdateData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1010, 2 },
                column: "RegistrationDate",
                value: new DateTime(2024, 4, 11, 2, 35, 17, 473, DateTimeKind.Local).AddTicks(5378));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Registrations");
        }
    }
}
