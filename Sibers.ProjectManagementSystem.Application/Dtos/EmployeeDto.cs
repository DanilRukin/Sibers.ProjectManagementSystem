using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Dtos
{
    public class EmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public List<Guid> ExecutableTasksIds { get; set; }
        public List<Guid> CreatedTasksIds { get; set; }
        public int Id { get; set; }
        public List<int> ProjectsIds { get; set; }
    }
}
