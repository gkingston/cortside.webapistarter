using Microsoft.EntityFrameworkCore.Migrations;

namespace Cortside.WebApiStarter.Data.Migrations
{
    public partial class Address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                schema: "dbo",
                table: "Widget",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorrelationId",
                schema: "dbo",
                table: "Outbox",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "dbo",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Widget_AddressId",
                schema: "dbo",
                table: "Widget",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Widget_Address_AddressId",
                schema: "dbo",
                table: "Widget",
                column: "AddressId",
                principalSchema: "dbo",
                principalTable: "Address",
                principalColumn: "AddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Widget_Address_AddressId",
                schema: "dbo",
                table: "Widget");

            migrationBuilder.DropTable(
                name: "Address",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Widget_AddressId",
                schema: "dbo",
                table: "Widget");

            migrationBuilder.DropColumn(
                name: "AddressId",
                schema: "dbo",
                table: "Widget");

            migrationBuilder.AlterColumn<string>(
                name: "CorrelationId",
                schema: "dbo",
                table: "Outbox",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
