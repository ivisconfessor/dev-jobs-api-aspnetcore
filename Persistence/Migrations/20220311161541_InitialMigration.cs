using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevJobs.API.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_VAGAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRemote = table.Column<bool>(type: "bit", nullable: false),
                    SalaryRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_VAGAS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_CANDIDATURAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdJobVacancy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CANDIDATURAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_CANDIDATURAS_TB_VAGAS_IdJobVacancy",
                        column: x => x.IdJobVacancy,
                        principalTable: "TB_VAGAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_IdJobVacancy",
                table: "TB_CANDIDATURAS",
                column: "IdJobVacancy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CANDIDATURAS");

            migrationBuilder.DropTable(
                name: "TB_VAGAS");
        }
    }
}
