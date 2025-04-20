using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmationRecipe.Migrations
{
    /// <inheritdoc />
    public partial class DeleteVarChangeToParametersInRecipeSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeStep_Description",
                table: "RecipeStep");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RecipeStep");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "RecipeStep");

            migrationBuilder.DropColumn(
                name: "Pressure",
                table: "RecipeStep");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "RecipeStep");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RecipeStep",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "RecipeStep",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Pressure",
                table: "RecipeStep",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Temperature",
                table: "RecipeStep",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeStep_Description",
                table: "RecipeStep",
                column: "Description");
        }
    }
}
