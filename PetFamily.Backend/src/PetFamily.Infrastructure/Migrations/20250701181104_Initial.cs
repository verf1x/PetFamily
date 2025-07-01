﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    species_breeds = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    total_years = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    help_requisites = table.Column<string>(type: "jsonb", nullable: false),
                    social_networks = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    health_status = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    height = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_neutered = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    help_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    volunteer_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    volunteer_id2 = table.Column<Guid>(type: "uuid", nullable: true),
                    volunteer_id3 = table.Column<Guid>(type: "uuid", nullable: false),
                    address_lines = table.Column<string>(type: "jsonb", nullable: false),
                    address_country_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    address_locality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address_postal_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address_region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    owner_phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    pet_help_requisites = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id1",
                        column: x => x.volunteer_id1,
                        principalTable: "volunteers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id2",
                        column: x => x.volunteer_id2,
                        principalTable: "volunteers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id3",
                        column: x => x.volunteer_id3,
                        principalTable: "volunteers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id",
                table: "pets",
                column: "volunteer_id");

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id1",
                table: "pets",
                column: "volunteer_id1");

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id2",
                table: "pets",
                column: "volunteer_id2");

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id3",
                table: "pets",
                column: "volunteer_id3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pets");

            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropTable(
                name: "volunteers");
        }
    }
}
