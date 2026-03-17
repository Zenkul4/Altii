using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "alti");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:audit_action", "create,update,delete,login,logout,check_in,check_out,payment,cancellation,room_block,room_release")
                .Annotation("Npgsql:Enum:audit_entity", "user,room,booking,payment,rate,season")
                .Annotation("Npgsql:Enum:booking_status", "pending_payment,confirmed,checked_in,checked_out,cancelled,expired")
                .Annotation("Npgsql:Enum:payment_status", "pending,approved,rejected,refunded")
                .Annotation("Npgsql:Enum:room_status", "available,occupied,cleaning,blocked,inactive")
                .Annotation("Npgsql:Enum:room_type", "single,double,suite,family,penthouse")
                .Annotation("Npgsql:Enum:user_role", "guest,receptionist,administrator")
                .Annotation("Npgsql:PostgresExtension:btree_gist", ",,");

            migrationBuilder.CreateTable(
                name: "additional_services",
                schema: "alti",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_additional_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                schema: "alti",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    floor = table.Column<short>(type: "smallint", nullable: false),
                    capacity = table.Column<short>(type: "smallint", nullable: false),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "alti",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    role = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                schema: "alti",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    executor_role = table.Column<string>(type: "text", nullable: true),
                    action = table.Column<string>(type: "text", nullable: false),
                    entity = table.Column<string>(type: "text", nullable: false),
                    entity_id = table.Column<int>(type: "integer", nullable: true),
                    previous_data = table.Column<string>(type: "jsonb", nullable: true),
                    new_data = table.Column<string>(type: "jsonb", nullable: true),
                    ip_address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    executed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_audit_logs_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "alti",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                schema: "alti",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    guest_id = table.Column<int>(type: "integer", nullable: false),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    attended_by_id = table.Column<int>(type: "integer", nullable: true),
                    check_in_date = table.Column<DateOnly>(type: "date", nullable: false),
                    check_out_date = table.Column<DateOnly>(type: "date", nullable: false),
                    price_per_night = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_bookings_rooms_room_id",
                        column: x => x.room_id,
                        principalSchema: "alti",
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_users_attended_by_id",
                        column: x => x.attended_by_id,
                        principalSchema: "alti",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_users_guest_id",
                        column: x => x.guest_id,
                        principalSchema: "alti",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "seasons",
                schema: "alti",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    multiplier = table.Column<decimal>(type: "numeric(4,2)", nullable: false, defaultValue: 1.00m),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seasons", x => x.id);
                    table.ForeignKey(
                        name: "FK_seasons_users_created_by_id",
                        column: x => x.created_by_id,
                        principalSchema: "alti",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "booking_services",
                schema: "alti",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    service_id = table.Column<int>(type: "integer", nullable: false),
                    registered_by_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<short>(type: "smallint", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    registered_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_services", x => x.id);
                    table.ForeignKey(
                        name: "FK_booking_services_additional_services_service_id",
                        column: x => x.service_id,
                        principalSchema: "alti",
                        principalTable: "additional_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_booking_services_bookings_booking_id",
                        column: x => x.booking_id,
                        principalSchema: "alti",
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_booking_services_users_registered_by_id",
                        column: x => x.registered_by_id,
                        principalSchema: "alti",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "alti",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    external_reference = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    processed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_bookings_booking_id",
                        column: x => x.booking_id,
                        principalSchema: "alti",
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rates",
                schema: "alti",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    season_id = table.Column<int>(type: "integer", nullable: false),
                    room_type = table.Column<string>(type: "text", nullable: false),
                    price_per_night = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rates", x => x.id);
                    table.ForeignKey(
                        name: "FK_rates_seasons_season_id",
                        column: x => x.season_id,
                        principalSchema: "alti",
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rates_users_created_by_id",
                        column: x => x.created_by_id,
                        principalSchema: "alti",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_additional_services_name",
                schema: "alti",
                table: "additional_services",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_entity_entity_id",
                schema: "alti",
                table: "audit_logs",
                columns: new[] { "entity", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_executed_at",
                schema: "alti",
                table: "audit_logs",
                column: "executed_at");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_user_id",
                schema: "alti",
                table: "audit_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_services_booking_id",
                schema: "alti",
                table: "booking_services",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_services_registered_by_id",
                schema: "alti",
                table: "booking_services",
                column: "registered_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_services_service_id",
                schema: "alti",
                table: "booking_services",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_attended_by_id",
                schema: "alti",
                table: "bookings",
                column: "attended_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_code",
                schema: "alti",
                table: "bookings",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_guest_id",
                schema: "alti",
                table: "bookings",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_room_id_check_in_date_check_out_date",
                schema: "alti",
                table: "bookings",
                columns: new[] { "room_id", "check_in_date", "check_out_date" });

            migrationBuilder.CreateIndex(
                name: "IX_payments_booking_id",
                schema: "alti",
                table: "payments",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_external_reference",
                schema: "alti",
                table: "payments",
                column: "external_reference",
                unique: true,
                filter: "external_reference IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_rates_created_by_id",
                schema: "alti",
                table: "rates",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_rates_season_id_room_type",
                schema: "alti",
                table: "rates",
                columns: new[] { "season_id", "room_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rooms_number",
                schema: "alti",
                table: "rooms",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seasons_created_by_id",
                schema: "alti",
                table: "seasons",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                schema: "alti",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs",
                schema: "alti");

            migrationBuilder.DropTable(
                name: "booking_services",
                schema: "alti");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "alti");

            migrationBuilder.DropTable(
                name: "rates",
                schema: "alti");

            migrationBuilder.DropTable(
                name: "additional_services",
                schema: "alti");

            migrationBuilder.DropTable(
                name: "bookings",
                schema: "alti");

            migrationBuilder.DropTable(
                name: "seasons",
                schema: "alti");

            migrationBuilder.DropTable(
                name: "rooms",
                schema: "alti");

            migrationBuilder.DropTable(
                name: "users",
                schema: "alti");
        }
    }
}
