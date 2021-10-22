using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramBot.DAL.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "isUnsubscribe",
            //    table: "Users",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isUnsubscribe",
                table: "Users");
        }
    }
}
