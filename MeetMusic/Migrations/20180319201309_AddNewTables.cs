using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MeetMusic.Migrations
{
    public partial class AddNewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MusicFamilies",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicFamilies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MusicGenres",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    FamilyId = table.Column<string>(maxLength: 36, nullable: true),
                    Name = table.Column<string>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusicGenres_MusicFamilies_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "MusicFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserMusicFamilies",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    FamilyId = table.Column<string>(maxLength: 36, nullable: false),
                    Rank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMusicFamilies", x => new { x.UserId, x.FamilyId });
                    table.ForeignKey(
                        name: "FK_UserMusicFamilies_MusicFamilies_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "MusicFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMusicFamilies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MusicGenres_FamilyId",
                table: "MusicGenres",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMusicFamilies_FamilyId",
                table: "UserMusicFamilies",
                column: "FamilyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicGenres");

            migrationBuilder.DropTable(
                name: "UserMusicFamilies");

            migrationBuilder.DropTable(
                name: "MusicFamilies");
        }
    }
}
