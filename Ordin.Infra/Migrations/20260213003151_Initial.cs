using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ordin.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_categories_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_transactions_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transactions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "email", "external_id", "name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), "master@master.com", "", "Master" });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "created_at", "name", "user_id" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), "Alimentação", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), "Transporte", new Guid("00000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "transactions",
                columns: new[] { "id", "amount", "category_id", "created_at", "date", "name", "type", "user_id" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), 55.50m, new Guid("11111111-1111-1111-1111-111111111111"), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 10, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), "Supermercado Continente", 1, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), 15.00m, new Guid("11111111-1111-1111-1111-111111111111"), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 11, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), "Almoço de Trabalho", 1, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("a3333333-3333-3333-3333-333333333333"), 4.20m, new Guid("11111111-1111-1111-1111-111111111111"), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), "Padaria", 1, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("b1111111-1111-1111-1111-111111111111"), 60.00m, new Guid("22222222-2222-2222-2222-222222222222"), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 7, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), "Combustível Galp", 1, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), 30.00m, new Guid("22222222-2222-2222-2222-222222222222"), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 2, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), "Passe Mensal Metro", 1, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), 12.50m, new Guid("22222222-2222-2222-2222-222222222222"), new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 9, 21, 21, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -3, 0, 0, 0)), "Uber para o Aeroporto", 1, new Guid("00000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.CreateIndex(
                name: "ix_categories_name_user_id",
                table: "categories",
                columns: new[] { "name", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_categories_user_id",
                table: "categories",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_category_id",
                table: "transactions",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_user_id",
                table: "transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_user_id_is_deleted_date",
                table: "transactions",
                columns: new[] { "user_id", "is_deleted", "date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
