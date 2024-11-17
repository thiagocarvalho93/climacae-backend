using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Climacae.Api.Migrations
{
    /// <inheritdoc />
    public partial class migracaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    StationId = table.Column<string>(type: "text", nullable: false),
                    ObsTimeLocal = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ObsTimeUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeZone = table.Column<string>(type: "text", nullable: true),
                    Epoch = table.Column<long>(type: "bigint", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    SolarRadiationHigh = table.Column<double>(type: "double precision", nullable: false),
                    UvHigh = table.Column<double>(type: "double precision", nullable: false),
                    WindDirectionAvg = table.Column<double>(type: "double precision", nullable: false),
                    HumidityHigh = table.Column<double>(type: "double precision", nullable: false),
                    HumidityLow = table.Column<double>(type: "double precision", nullable: false),
                    HumidityAvg = table.Column<double>(type: "double precision", nullable: false),
                    QualityControlStatus = table.Column<int>(type: "integer", nullable: false),
                    TempHigh = table.Column<double>(type: "double precision", nullable: false),
                    TempLow = table.Column<double>(type: "double precision", nullable: false),
                    TempAvg = table.Column<double>(type: "double precision", nullable: false),
                    WindspeedHigh = table.Column<double>(type: "double precision", nullable: false),
                    WindspeedLow = table.Column<double>(type: "double precision", nullable: false),
                    WindspeedAvg = table.Column<double>(type: "double precision", nullable: false),
                    WindGustHigh = table.Column<double>(type: "double precision", nullable: false),
                    WindGustLow = table.Column<double>(type: "double precision", nullable: false),
                    WindGustAvg = table.Column<double>(type: "double precision", nullable: false),
                    DewPointHigh = table.Column<double>(type: "double precision", nullable: false),
                    DewPointLow = table.Column<double>(type: "double precision", nullable: false),
                    DewPointAvg = table.Column<double>(type: "double precision", nullable: false),
                    WindChillHigh = table.Column<double>(type: "double precision", nullable: false),
                    WindChillLow = table.Column<double>(type: "double precision", nullable: false),
                    WindChillAvg = table.Column<double>(type: "double precision", nullable: false),
                    HeatIndexHigh = table.Column<double>(type: "double precision", nullable: false),
                    HeatIndexLow = table.Column<double>(type: "double precision", nullable: false),
                    HeatIndexAvg = table.Column<double>(type: "double precision", nullable: false),
                    PressureMax = table.Column<double>(type: "double precision", nullable: false),
                    PressureMin = table.Column<double>(type: "double precision", nullable: false),
                    PressureTrend = table.Column<double>(type: "double precision", nullable: false),
                    PrecipitationRate = table.Column<double>(type: "double precision", nullable: false),
                    PrecipitationTotal = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => new { x.StationId, x.ObsTimeLocal });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Observations");
        }
    }
}
