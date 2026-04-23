using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordin.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ImproveIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_transactions_category_id",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "ix_transactions_user_id",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "ix_categories_name_user_id",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "ix_categories_user_id",
                table: "categories");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_category_id_is_deleted_date",
                table: "transactions",
                columns: new[] { "category_id", "is_deleted", "date" });

            migrationBuilder.CreateIndex(
                name: "ix_categories_user_id_is_deleted_name",
                table: "categories",
                columns: new[] { "user_id", "is_deleted", "name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_transactions_category_id_is_deleted_date",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "ix_categories_user_id_is_deleted_name",
                table: "categories");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_category_id",
                table: "transactions",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_user_id",
                table: "transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_name_user_id",
                table: "categories",
                columns: new[] { "name", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_categories_user_id",
                table: "categories",
                column: "user_id");
        }
    }
}
