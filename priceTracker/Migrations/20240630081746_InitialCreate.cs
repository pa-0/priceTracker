using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace priceTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: true),
                    mail = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: true),
                    empType = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: true),
                    IsMail = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employe__3213E83F0B679910", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entrys",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    startDate = table.Column<DateOnly>(type: "date", nullable: true),
                    finishDate = table.Column<DateOnly>(type: "date", nullable: true),
                    recordDate = table.Column<DateOnly>(type: "date", nullable: true),
                    empId = table.Column<int>(type: "int", nullable: true),
                    url1 = table.Column<string>(type: "nchar(300)", fixedLength: true, maxLength: 300, nullable: true),
                    url2 = table.Column<string>(type: "nchar(300)", fixedLength: true, maxLength: 300, nullable: true),
                    url3 = table.Column<string>(type: "nchar(300)", fixedLength: true, maxLength: 300, nullable: true),
                    url4 = table.Column<string>(type: "nchar(300)", fixedLength: true, maxLength: 300, nullable: true),
                    url5 = table.Column<string>(type: "nchar(300)", fixedLength: true, maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Entry__3213E83F69EE01BF", x => x.id);
                    table.ForeignKey(
                        name: "FK_Entry_Employe",
                        column: x => x.empId,
                        principalTable: "employes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prodId = table.Column<int>(type: "int", nullable: true),
                    urlNumber = table.Column<int>(type: "int", nullable: true),
                    productName = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: true),
                    siteName = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: true),
                    url = table.Column<string>(type: "nchar(300)", fixedLength: true, maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__3213E83F59A3E981", x => x.id);
                    table.ForeignKey(
                        name: "fk_prod_entry",
                        column: x => x.prodId,
                        principalTable: "entrys",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_entrys_empId",
                table: "entrys",
                column: "empId");

            migrationBuilder.CreateIndex(
                name: "IX_products_prodId",
                table: "products",
                column: "prodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "entrys");

            migrationBuilder.DropTable(
                name: "employes");
        }
    }
}
