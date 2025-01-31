using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceVoyage.Migrations
{
    /// <inheritdoc />
    public partial class TestovaciStlpecRemovedFilePathhAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "PatchNotes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "PatchNotes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PatchNotes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "PatchNotes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "PatchNotes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "PatchNotes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PatchNotes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "PatchNotes",
                type: "BLOB",
                nullable: true);
        }
    }
}
