using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updateleadsandcustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Leads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Customers");
        }
    }
}
