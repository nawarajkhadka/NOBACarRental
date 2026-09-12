using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingAuditColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Booking",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CreatedBy",
                table: "Booking",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UpdatedBy",
                table: "Booking",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_CreatedBy",
                table: "Booking",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_UpdatedBy",
                table: "Booking",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_CreatedBy",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_UpdatedBy",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_CreatedBy",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_UpdatedBy",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Booking");
        }
    }
}
