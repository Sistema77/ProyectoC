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
            migrationBuilder.EnsureSchema(
                name: "schemabody");

            migrationBuilder.EnsureSchema(
                name: "schemausuario");

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "schemausuario",
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
                    table.PrimaryKey("PK_Usuario", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "Cuenta",
                schema: "schemabody",
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
                    table.PrimaryKey("PK_Cuenta", x => x.id_cuenta);
                    table.ForeignKey(
                        name: "FK_Cuenta_Usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "schemausuario",
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credito",
                schema: "schemabody",
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
                    table.PrimaryKey("PK_Credito", x => x.id_credito);
                    table.ForeignKey(
                        name: "FK_Credito_Cuenta_id_cuenta",
                        column: x => x.id_cuenta,
                        principalSchema: "schemabody",
                        principalTable: "Cuenta",
                        principalColumn: "id_cuenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaccion",
                schema: "schemabody",
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
                    table.PrimaryKey("PK_Transaccion", x => x.id_transaccion);
                    table.ForeignKey(
                        name: "FK_Transaccion_Cuenta_id_cuenta",
                        column: x => x.id_cuenta,
                        principalSchema: "schemabody",
                        principalTable: "Cuenta",
                        principalColumn: "id_cuenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credito_id_cuenta",
                schema: "schemabody",
                table: "Credito",
                column: "id_cuenta");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_id_usuario",
                schema: "schemabody",
                table: "Cuenta",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_id_cuenta",
                schema: "schemabody",
                table: "Transaccion",
                column: "id_cuenta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credito",
                schema: "schemabody");

            migrationBuilder.DropTable(
                name: "Transaccion",
                schema: "schemabody");

            migrationBuilder.DropTable(
                name: "Cuenta",
                schema: "schemabody");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "schemausuario");
        }
    }
}
