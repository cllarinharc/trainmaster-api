using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TrainMaster.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseActivityProgressEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Score = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    StartedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastAccessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseActivityProgressEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseActivityProgressEntity_CourseActivitieEntity_Activity~",
                        column: x => x.ActivityId,
                        principalTable: "CourseActivitieEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseActivityProgressEntity_CourseEntity_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseActivityProgressEntity_UserEntity_StudentId",
                        column: x => x.StudentId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseProgressEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    ProgressPercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CompletedActivitiesCount = table.Column<int>(type: "integer", nullable: false),
                    TotalActivitiesCount = table.Column<int>(type: "integer", nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastActivityId = table.Column<int>(type: "integer", nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseProgressEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseProgressEntity_CourseActivitieEntity_LastActivityId",
                        column: x => x.LastActivityId,
                        principalTable: "CourseActivitieEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CourseProgressEntity_CourseEntity_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseProgressEntity_UserEntity_StudentId",
                        column: x => x.StudentId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseActivityProgressEntity_ActivityId",
                table: "CourseActivityProgressEntity",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseActivityProgressEntity_CourseId",
                table: "CourseActivityProgressEntity",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseActivityProgressEntity_StudentId_ActivityId",
                table: "CourseActivityProgressEntity",
                columns: new[] { "StudentId", "ActivityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgressEntity_CourseId",
                table: "CourseProgressEntity",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgressEntity_LastActivityId",
                table: "CourseProgressEntity",
                column: "LastActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgressEntity_StudentId_CourseId",
                table: "CourseProgressEntity",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseActivityProgressEntity");

            migrationBuilder.DropTable(
                name: "CourseProgressEntity");
        }
    }
}
