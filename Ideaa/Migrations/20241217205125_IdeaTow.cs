using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ideaa.Migrations
{
    /// <inheritdoc />
    public partial class IdeaTow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdeaSupervisorId",
                table: "IdeaSupervisors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdeaSupervisorId",
                table: "IdeaSupervisors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
