using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAKIBPORTFOLIO.Data.Migrations
{
    /// <inheritdoc />
    public partial class TABLEADD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MY_PROFILE",
                columns: table => new
                {
                    AUTOID = table.Column<int>(name: "AUTO_ID", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MYNAME = table.Column<string>(name: "MY_NAME", type: "nvarchar(max)", nullable: true),
                    DESIGNATION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AGE = table.Column<int>(type: "int", nullable: false),
                    MYWEBSITE = table.Column<string>(name: "MY_WEBSITE", type: "nvarchar(max)", nullable: true),
                    DEGREE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PHONE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CURRENTCITY = table.Column<string>(name: "CURRENT_CITY", type: "nvarchar(max)", nullable: true),
                    HOMETOWN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DES1 = table.Column<string>(name: "DES_1", type: "nvarchar(max)", nullable: true),
                    DES2 = table.Column<string>(name: "DES_2", type: "nvarchar(max)", nullable: true),
                    DES3 = table.Column<string>(name: "DES_3", type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MY_PROFILE", x => x.AUTOID);
                });

            migrationBuilder.CreateTable(
                name: "MY_SKILLS",
                columns: table => new
                {
                    AUTOID = table.Column<int>(name: "AUTO_ID", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SKILLNAME = table.Column<string>(name: "SKILL_NAME", type: "nvarchar(max)", nullable: true),
                    SKILLPERCENTAGE = table.Column<int>(name: "SKILL_PERCENTAGE", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MY_SKILLS", x => x.AUTOID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MY_PROFILE");

            migrationBuilder.DropTable(
                name: "MY_SKILLS");
        }
    }
}
