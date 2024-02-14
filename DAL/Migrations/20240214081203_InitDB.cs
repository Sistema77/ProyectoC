using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuarioDAO",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dni = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    expiracion_token = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fch_alta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    foto = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    tipo_usuario = table.Column<string>(type: "text", nullable: false),
                    tlf = table.Column<string>(type: "text", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    cuentaConfirmada = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarioDAO", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "cuentaDAO",
                columns: table => new
                {
                    id_cuenta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    con_nomina = table.Column<bool>(type: "boolean", nullable: false),
                    fch_apertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    numero_cuenta = table.Column<string>(type: "text", nullable: false),
                    saldo = table.Column<decimal>(type: "numeric", nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cuentaDAO", x => x.id_cuenta);
                    table.ForeignKey(
                        name: "FK_cuentaDAO_usuarioDAO_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuarioDAO",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "creditoDAO",
                columns: table => new
                {
                    id_credito = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cantidad_prestamo = table.Column<decimal>(type: "numeric", nullable: false),
                    cuota_mensual = table.Column<decimal>(type: "numeric", nullable: false),
                    estado_prestamo = table.Column<string>(type: "text", nullable: false),
                    fch_final = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fch_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tasa_interes = table.Column<decimal>(type: "numeric", nullable: false),
                    tipo_prestamo = table.Column<string>(type: "text", nullable: false),
                    id_cuenta = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_creditoDAO", x => x.id_credito);
                    table.ForeignKey(
                        name: "FK_creditoDAO_cuentaDAO_id_cuenta",
                        column: x => x.id_cuenta,
                        principalTable: "cuentaDAO",
                        principalColumn: "id_cuenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaccionDAO",
                columns: table => new
                {
                    id_transaccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fch_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cantidad_dinero = table.Column<decimal>(type: "numeric", nullable: false),
                    TipoTransacion = table.Column<string>(type: "text", nullable: false),
                    NumeroTrasaccion = table.Column<long>(type: "bigint", nullable: false),
                    id_cuenta = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaccionDAO", x => x.id_transaccion);
                    table.ForeignKey(
                        name: "FK_transaccionDAO_cuentaDAO_id_cuenta",
                        column: x => x.id_cuenta,
                        principalTable: "cuentaDAO",
                        principalColumn: "id_cuenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_creditoDAO_id_cuenta",
                table: "creditoDAO",
                column: "id_cuenta");

            migrationBuilder.CreateIndex(
                name: "IX_cuentaDAO_id_usuario",
                table: "cuentaDAO",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_transaccionDAO_id_cuenta",
                table: "transaccionDAO",
                column: "id_cuenta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "creditoDAO");

            migrationBuilder.DropTable(
                name: "transaccionDAO");

            migrationBuilder.DropTable(
                name: "cuentaDAO");

            migrationBuilder.DropTable(
                name: "usuarioDAO");
        }
    }
}
