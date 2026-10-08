using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revemar.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCamposAuditoriaVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "VEICULOS",
                type: "TIMESTAMP(7)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "VEICULOS");
        }
    }
}
