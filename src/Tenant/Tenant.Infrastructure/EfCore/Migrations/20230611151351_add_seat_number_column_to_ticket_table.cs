using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenant.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class add_seat_number_column_to_ticket_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "passengers");

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "tickets",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "tickets");

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "passengers",
                type: "text",
                nullable: true);
        }
    }
}
