using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.DataAccess
{
    public static class ProjectManagementSystemContextExtensions
    {
        
        public static IQueryable<Project> IncludeTasks(this IQueryable<Project> projects)
        {
            return projects.Include(DataAccessConstants.Tasks);
        }

        public static IQueryable<Project> IncludeEmployees(this IQueryable<Project> projects)
        {
            return projects.Include(DataAccessConstants.EmployeesOnProject);
        }

        public static IQueryable<Employee> IncludeProjects(this IQueryable<Employee> employees)
        {
            return employees.Include(DataAccessConstants.EmployeeOnProjects);
        }

        public static IQueryable<Employee> IncludeExecutableTasks(this IQueryable<Employee> employees)
        {
            return employees.Include(DataAccessConstants.ExecutableTasks);
        }

        public static IQueryable<Employee> IncludeCreatedTasks(this IQueryable<Employee> employees)
        {
            return employees.Include(DataAccessConstants.CreatedTasks);
        }

        public static IQueryable<Employee> IncludeTasks(this IQueryable<Employee> employees)
        {
            return employees.IncludeExecutableTasks().IncludeCreatedTasks();
        }
    }
}
