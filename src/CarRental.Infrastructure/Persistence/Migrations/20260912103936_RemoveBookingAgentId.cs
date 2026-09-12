using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookingAgentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_AgentId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_AgentId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Booking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_AgentId",
                table: "Booking",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_AgentId",
                table: "Booking",
                column: "AgentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
