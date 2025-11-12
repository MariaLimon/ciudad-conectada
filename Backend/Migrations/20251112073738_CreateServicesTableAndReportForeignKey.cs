using Microsoft.EntityFrameworkCore.Migrations;
using System;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateServicesTableAndReportForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
{
    // PASO 1: Crear la tabla Services.
    migrationBuilder.CreateTable(
        name: "Services",
        columns: table => new
        {
            Id = table.Column<int>(type: "integer", nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            Company = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Services", x => x.Id);
        });

    // PASO 2: Crear un índice ÚNICO en la columna Type.
    // ¡ESTO ES ESENCIAL para que ON CONFLICT funcione!
    migrationBuilder.CreateIndex(
        name: "IX_Services_Type",
        table: "Services",
        column: "Type",
        unique: true);

    // PASO 3: Añadir la nueva columna ServiceId como NULLABLE.
    migrationBuilder.AddColumn<int>(
        name: "ServiceId",
        table: "Reports",
        type: "integer",
        nullable: true);

    // PASO 4: Poblar la tabla Services con los tipos únicos de Reports.
    migrationBuilder.Sql(@"
        INSERT INTO ""Services"" (""Type"", ""Company"")
        SELECT DISTINCT ""TypeService"", 'Empresa Municipal'
        FROM ""Reports""
        WHERE ""TypeService"" IS NOT NULL AND ""TypeService"" <> ''
        ON CONFLICT (""Type"") DO NOTHING;
    ");

    // PASO 5: Actualizar la nueva columna ServiceId con el ID correcto.
    migrationBuilder.Sql(@"
        UPDATE ""Reports""
        SET ""ServiceId"" = s.""Id""
        FROM ""Services"" s
        WHERE ""Reports"".""TypeService"" = s.""Type"";
    ");

    // PASO 6: Hacer ServiceId obligatorio (NOT NULL).
    migrationBuilder.AlterColumn<int>(
        name: "ServiceId",
        table: "Reports",
        type: "integer",
        nullable: false);

    // PASO 7: Eliminar la vieja columna TypeService.
    migrationBuilder.DropColumn(
        name: "TypeService",
        table: "Reports");
}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
