using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebExcelReader.Migrations
{
    /// <inheritdoc />
    public partial class AddingExcelReaderToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExcelReaderTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lowest_probability_value = table.Column<double>(type: "float", nullable: true),
                    highest_probability_value = table.Column<double>(type: "float", nullable: true),
                    lowest_probability_country_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    highest_probability_country_code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelReaderTable", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcelReaderTable");
        }
    }
}
