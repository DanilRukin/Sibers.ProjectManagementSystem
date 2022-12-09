using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services.Interfaces;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Services.Mappers
{
    public class EmployeeToEmployeeDtoMapper : IMapper<Employee, EmployeeDto>
    {
        public EmployeeDto Map(Employee source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            EmployeeDto result = new EmployeeDto
            {
                Email = source.Email.Value,
                FirstName = source.PersonalData.FirstName,
                LastName = source.PersonalData.LastName,
                Patronymic = source.PersonalData.Patronymic,
                Id = source.Id,
            };
            var createdTasks = source.CreatedTasks?.ToArray();
            if (createdTasks != null)
                result.CreatedTasksIds = createdTasks.Select(t => t.Id).ToList();
            else
                result.CreatedTasksIds = new List<Guid>();
            var executableTasks = source.ExecutableTasks?.ToArray();
            if (executableTasks != null)
                result.ExecutableTasksIds = executableTasks.Select(t => t.Id).ToList();
            else
                result.ExecutableTasksIds = new List<Guid>();
            return result;
        }
    }
}
