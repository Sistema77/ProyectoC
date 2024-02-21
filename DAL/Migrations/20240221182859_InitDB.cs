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
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dni = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    expiracion_token = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fch_alta = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    foto = table.Column<byte[]>(type: "bytea", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    tipo_usuario = table.Column<string>(type: "text", nullable: true),
                    tlf = table.Column<string>(type: "text", nullable: false),
                    token = table.Column<string>(type: "text", nullable: true),
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
                    id_cuenta = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    con_nomina = table.Column<bool>(type: "boolean", nullable: false),
                    fch_apertura = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    numero_cuenta = table.Column<string>(type: "text", nullable: false),
                    saldo = table.Column<decimal>(type: "numeric", nullable: false),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
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
                name: "creditoDAO",
                columns: table => new
                {
                    id_credito = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cantidad_prestamo = table.Column<decimal>(type: "numeric", nullable: false),
                    cuota_mensual = table.Column<decimal>(type: "numeric", nullable: false),
                    estado_prestamo = table.Column<string>(type: "text", nullable: false),
                    fch_final = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fch_inicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    tasa_interes = table.Column<decimal>(type: "numeric", nullable: false),
                    tipo_prestamo = table.Column<string>(type: "text", nullable: false),
                    id_cuenta = table.Column<long>(type: "bigint", nullable: false),
                    Cuentaid_cuenta = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_creditoDAO", x => x.id_credito);
                    table.ForeignKey(
                        name: "FK_creditoDAO_Cuenta_Cuentaid_cuenta",
                        column: x => x.Cuentaid_cuenta,
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
                    id_transaccion = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fch_hora = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    cantidad_dinero = table.Column<decimal>(type: "numeric", nullable: false),
                    TipoTransacion = table.Column<string>(type: "text", nullable: false),
                    NumeroTrasaccion = table.Column<long>(type: "bigint", nullable: false),
                    id_cuenta = table.Column<long>(type: "bigint", nullable: false),
                    Cuentaid_cuenta = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion", x => x.id_transaccion);
                    table.ForeignKey(
                        name: "FK_Transaccion_Cuenta_Cuentaid_cuenta",
                        column: x => x.Cuentaid_cuenta,
                        principalSchema: "schemabody",
                        principalTable: "Cuenta",
                        principalColumn: "id_cuenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_creditoDAO_Cuentaid_cuenta",
                table: "creditoDAO",
                column: "Cuentaid_cuenta");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_id_usuario",
                schema: "schemabody",
                table: "Cuenta",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Cuentaid_cuenta",
                schema: "schemabody",
                table: "Transaccion",
                column: "Cuentaid_cuenta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "creditoDAO");

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
