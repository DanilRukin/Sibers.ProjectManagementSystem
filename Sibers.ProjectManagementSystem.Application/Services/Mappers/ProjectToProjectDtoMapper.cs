using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services.Interfaces;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Services.Mappers
{
    public class ProjectToProjectDtoMapper : IMapper<Project, ProjectDto>
    {
        public ProjectDto Map(Project source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            ProjectDto result = new ProjectDto();
            result.Id = source.Id;
            result.Priority = source.Priority.Value;
            result.Name = source.Name ?? "";
            result.NameOfTheContractorCompany = source.NameOfTheContractorCompany ?? "";
            result.NameOfTheCustomerCompany = source.NameOfTheCustomerCompany ?? "";
            result.StartDate = source.StartDate;
            result.EndDate = source.EndDate;
            var tasks = source.Tasks?.ToArray();
            if (tasks != null)
                result.TasksIds = tasks.Select(t => t.Id).ToList();
            else
                result.TasksIds = new List<Guid>();
            Employee[]? employees = source.Employees?.ToArray();
            if (employees != null)
                result.EmployeesIds = employees.Select(e => e.Id).ToList();
            else
                result.EmployeesIds = new List<int>();
            Employee? manager = source.Manager;
            if (manager != null)
                result.ManagerId = manager.Id;
            else
                result.ManagerId = 0;
            return result;
        }
    }
}
