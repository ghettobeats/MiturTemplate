using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiturNETInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TableTransporte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NombreEstado = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estado__3214EC071945AB43", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosChofer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    IdEstadoNavigationId = table.Column<int>(type: "int", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosChofer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstadosChofer_Estado_IdEstadoNavigationId",
                        column: x => x.IdEstadoNavigationId,
                        principalTable: "Estado",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EstadoSolicitud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EstadoSo__3214EC07185155CA", x => x.Id);
                    table.ForeignKey(
                        name: "FK__EstadoSol__IdEst__00200768",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EstadoVehiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EstadoVe__3214EC07DF46176B", x => x.Id);
                    table.ForeignKey(
                        name: "FK__EstadoVeh__IdEst__01142BA1",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chofer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdEmpleado = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Cedula = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdEstadoChofer = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    EsConductor = table.Column<bool>(type: "bit", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Chofer__3214EC07984A763F", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Chofer__IdEstado__7D439ABD",
                        column: x => x.IdEstadoChofer,
                        principalTable: "EstadosChofer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Chofer__IdEstado__7E37BEF6",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Solicitud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    Destino = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CantidadPasajeros = table.Column<int>(type: "int", nullable: true),
                    Adjunto = table.Column<byte[]>(type: "binary(1)", fixedLength: true, maxLength: 1, nullable: true),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Proposito = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Comentario = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdEstadoSolicitud = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Solicitu__3214EC07571202BD", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Solicitud__IdEst__02084FDA",
                        column: x => x.IdEstadoSolicitud,
                        principalTable: "EstadoSolicitud",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Solicitud__IdEst__02FC7413",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vehiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Marca = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Placa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdChofer = table.Column<int>(type: "int", nullable: true),
                    IdEstadoVehiculo = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vehiculo__3214EC076A1346CD", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Vehiculo__IdChof__06CD04F7",
                        column: x => x.IdChofer,
                        principalTable: "Chofer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Vehiculo__IdEsta__07C12930",
                        column: x => x.IdEstadoVehiculo,
                        principalTable: "EstadoVehiculo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Vehiculo__IdEsta__08B54D69",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SolicitudDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdSolicitud = table.Column<int>(type: "int", nullable: true),
                    IdPasajero = table.Column<int>(type: "int", nullable: true),
                    IdChofer = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    IdVehiculo = table.Column<int>(type: "int", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Solicitu__3214EC07FC81005B", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudDetalle_Chofer",
                        column: x => x.IdChofer,
                        principalTable: "Chofer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Solicitud__IdEst__03F0984C",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Solicitud__IdSol__04E4BC85",
                        column: x => x.IdSolicitud,
                        principalTable: "Solicitud",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chofer_IdEstado",
                table: "Chofer",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Chofer_IdEstadoChofer",
                table: "Chofer",
                column: "IdEstadoChofer");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosChofer_IdEstadoNavigationId",
                table: "EstadosChofer",
                column: "IdEstadoNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoSolicitud_IdEstado",
                table: "EstadoSolicitud",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoVehiculo_IdEstado",
                table: "EstadoVehiculo",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_IdEstado",
                table: "Solicitud",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_IdEstadoSolicitud",
                table: "Solicitud",
                column: "IdEstadoSolicitud");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudDetalle_IdChofer",
                table: "SolicitudDetalle",
                column: "IdChofer");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudDetalle_IdEstado",
                table: "SolicitudDetalle",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudDetalle_IdSolicitud",
                table: "SolicitudDetalle",
                column: "IdSolicitud");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculo_IdChofer",
                table: "Vehiculo",
                column: "IdChofer");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculo_IdEstado",
                table: "Vehiculo",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculo_IdEstadoVehiculo",
                table: "Vehiculo",
                column: "IdEstadoVehiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudDetalle");

            migrationBuilder.DropTable(
                name: "Vehiculo");

            migrationBuilder.DropTable(
                name: "Solicitud");

            migrationBuilder.DropTable(
                name: "Chofer");

            migrationBuilder.DropTable(
                name: "EstadoVehiculo");

            migrationBuilder.DropTable(
                name: "EstadoSolicitud");

            migrationBuilder.DropTable(
                name: "EstadosChofer");

            migrationBuilder.DropTable(
                name: "Estado");
        }
    }
}
