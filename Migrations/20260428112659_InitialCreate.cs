using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace temperature_system.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemperatureReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TemperatureCelsius = table.Column<double>(type: "REAL", nullable: false),
                    Humidity = table.Column<double>(type: "REAL", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureReadings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemperatureReadings_DeviceId_Timestamp",
                table: "TemperatureReadings",
                columns: new[] { "DeviceId", "Timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemperatureReadings");
        }
    }
}
