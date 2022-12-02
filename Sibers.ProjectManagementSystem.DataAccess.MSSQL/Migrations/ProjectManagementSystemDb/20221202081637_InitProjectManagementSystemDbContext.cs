using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibers.ProjectManagementSystem.DataAccess.MSSQL.Migrations.ProjectManagementSystemDb
{
    /// <inheritdoc />
    public partial class InitProjectManagementSystemDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalDataFirstName = table.Column<string>(name: "PersonalData_FirstName", type: "nvarchar(max)", nullable: false),
                    PersonalDataLastName = table.Column<string>(name: "PersonalData_LastName", type: "nvarchar(max)", nullable: false),
                    PersonalDataPatronymic = table.Column<string>(name: "PersonalData_Patronymic", type: "nvarchar(max)", nullable: false),
                    EmailValue = table.Column<string>(name: "Email_Value", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriorityValue = table.Column<int>(name: "Priority_Value", type: "int", nullable: false),
                    NameOfTheCustomerCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOfTheContractorCompany = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesOnProjects",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Employee")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesOnProjects", x => new { x.ProjectId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeesOnProjects_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeesOnProjects_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityValue = table.Column<int>(name: "Priority_Value", type: "int", nullable: false),
                    TaskStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "ToDo"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ContractorEmployeeId = table.Column<int>(type: "int", nullable: true),
                    AuthorEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task_Employees_AuthorEmployeeId",
                        column: x => x.AuthorEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Task_Employees_ContractorEmployeeId",
                        column: x => x.ContractorEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Task_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesOnProjects_EmployeeId",
                table: "EmployeesOnProjects",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_AuthorEmployeeId",
                table: "Task",
                column: "AuthorEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ContractorEmployeeId",
                table: "Task",
                column: "ContractorEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ProjectId",
                table: "Task",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeesOnProjects");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
