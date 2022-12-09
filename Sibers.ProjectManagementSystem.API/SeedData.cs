using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.Services;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;

namespace Sibers.ProjectManagementSystem.API
{
    public static class SeedData
    {
        private static bool _empty = false;
        public static Project Project1 { get; private set; }
        public static Project Project2 { get; private set; }

        public static Employee Employee1_WorksOnProject1 { get; private set; }
        public static Employee Employee2_WorksOnProject1And2 { get; private set; }
        public static Employee Employee3_WorksOnProject2 { get; private set; }

        public static Task TaskByEmployee1_OnProject1_Employee2_Executor { get; private set; }
        public static Task TaskByEmployee2_OnProject2_Employee3_Executor { get; private set; }
        public static Task TaskByEmployee1_OnProject2_Employee2_Executor { get; private set; }

        public static void ApplyMigrationAndFillDatabase(ProjectManagementSystemContext context)
        {
            context.Database.Migrate();
            ClearDatabase(context);
            FillDatabase(context);
        }

        public static void InitializeDatabase(ProjectManagementSystemContext context)
        {
            context.Database.EnsureCreated();
            ClearDatabase(context);
            FillDatabase(context);
        }

        private static void FillDatabase(ProjectManagementSystemContext context)
        {
            if (_empty)
            {
                ProjectFactory projectFactory = new ProjectFactory();
                EmployeeFactory employeeFactory = new EmployeeFactory();
                DateTime startDate = DateTime.Today;
                DateTime endDate = startDate.AddDays(1);

                Project1 = projectFactory.CreateProject(nameof(Project1), startDate, endDate,
                    $"{nameof(Project1)}_contractorCompany", $"{nameof(Project1)}_customerCompany");
                Project2 = projectFactory.CreateProject(nameof(Project2), startDate, endDate,
                    $"{nameof(Project2)}_contractorCompany", $"{nameof(Project2)}_customerCompany");
                context.Projects.AddRange(Project1, Project2);
                context.SaveChanges();

                Employee1_WorksOnProject1 = employeeFactory.CreateEmployee($"{nameof(Employee1_WorksOnProject1)}@gmail.com",
                    "Ivanov", "Ivan", "Ivanovich");
                Employee2_WorksOnProject1And2 = employeeFactory.CreateEmployee($"{nameof(Employee2_WorksOnProject1And2)}@gmail.com",
                    "Petrov", "Ivan", "Petrovich");
                Employee3_WorksOnProject2 = employeeFactory.CreateEmployee($"{nameof(Employee3_WorksOnProject2)}@gmail.com",
                    "Sidorov", "Petr", "Ivanovich");
                
                context.Employees.AddRange(Employee1_WorksOnProject1, Employee2_WorksOnProject1And2, Employee3_WorksOnProject2);
                context.SaveChanges();
                Project1.AddEmployee(Employee1_WorksOnProject1);
                Project1.AddEmployee(Employee2_WorksOnProject1And2);
                Project2.AddEmployee(Employee2_WorksOnProject1And2);
                Project2.AddEmployee(Employee3_WorksOnProject2);
                context.SaveChanges();

                TaskByEmployee1_OnProject1_Employee2_Executor = Employee1_WorksOnProject1.CreateTask(Project1, "name");
                Project1.ChangeTasksContractor(TaskByEmployee1_OnProject1_Employee2_Executor, Employee2_WorksOnProject1And2);
                TaskByEmployee1_OnProject2_Employee2_Executor = Employee1_WorksOnProject1.CreateTask(Project2, "name");
                Project2.ChangeTasksContractor(TaskByEmployee1_OnProject2_Employee2_Executor, Employee2_WorksOnProject1And2);
                TaskByEmployee2_OnProject2_Employee3_Executor = Employee2_WorksOnProject1And2.CreateTask(Project2, "name");
                Project2.ChangeTasksContractor(TaskByEmployee2_OnProject2_Employee3_Executor, Employee3_WorksOnProject2);

                context.SaveChanges();

                _empty = false;
            }
        }

        private static void ClearDatabase(ProjectManagementSystemContext context)
        {
            if (context.Projects.Any())
                context.Projects.RemoveRange(context.Projects);
            if (context.Tasks.Any())
                context.Tasks.RemoveRange(context.Tasks);
            if (context.Employees.Any())
                context.Employees.RemoveRange(context.Employees);
            if (context.EmployeesOnProjects.Any())
                context.EmployeesOnProjects.RemoveRange(context.EmployeesOnProjects);
            context.SaveChanges();
            Project1 = null;
            Project2 = null;

            Employee1_WorksOnProject1 = null;
            Employee2_WorksOnProject1And2 = null;
            Employee3_WorksOnProject2 = null;

            TaskByEmployee1_OnProject1_Employee2_Executor = null;
            TaskByEmployee2_OnProject2_Employee3_Executor = null;
            TaskByEmployee1_OnProject2_Employee2_Executor = null;
            _empty = true;
        }

        
    }
}
