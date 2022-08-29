using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altairis.Services.StaticContent.DemoApp.Migrations
{
    public partial class Add_DefaultContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StaticContentItems",
                columns: new[] { "Key", "Text" },
                values: new object[] { "privacy", "This is my _privacy policy_." });

            migrationBuilder.InsertData(
                table: "StaticContentItems",
                columns: new[] { "Key", "Text" },
                values: new object[] { "welcome", "# Welcome" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StaticContentItems",
                keyColumn: "Key",
                keyValue: "privacy");

            migrationBuilder.DeleteData(
                table: "StaticContentItems",
                keyColumn: "Key",
                keyValue: "welcome");
        }
    }
}
