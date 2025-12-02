using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class modificacionSubirEvidencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Evidencia",
                table: "Reports");

            migrationBuilder.AddColumn<List<string>>(
                name: "Evidencias",
                table: "Reports",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Evidencias",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "Evidencia",
                table: "Reports",
                type: "text",
                nullable: true);
        }
    }
}
