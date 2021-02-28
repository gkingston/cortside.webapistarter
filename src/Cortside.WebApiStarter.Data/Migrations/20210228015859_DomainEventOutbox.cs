using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cortside.WebApiStarter.Data.Migrations
{
    public partial class DomainEventOutbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Outbox",
                schema: "dbo",
                columns: table => new
                {
                    MessageId = table.Column<string>(nullable: false),
                    CorrelationId = table.Column<string>(nullable: true),
                    EventType = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ScheduledDate = table.Column<DateTime>(nullable: false),
                    PublishedDate = table.Column<DateTime>(nullable: true),
                    LockId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outbox", x => x.MessageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDate_Status",
                schema: "dbo",
                table: "Outbox",
                columns: new[] { "ScheduledDate", "Status" })
                .Annotation("SqlServer:Include", new[] { "EventType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outbox",
                schema: "dbo");
        }
    }
}
