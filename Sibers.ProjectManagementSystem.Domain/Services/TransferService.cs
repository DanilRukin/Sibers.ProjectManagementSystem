using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate.Specifications;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Specifications;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.Services
{
    public class TransferService : ITransferService
    {
        private IRepository<Project> _projectRepository;
        private IRepository<Employee> _employeeRepository;

        public TransferService(IRepository<Project> projectRepository, IRepository<Employee> employeeRepository)
        {
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task AddEmployeeToProject(int employeeId, int projectId, CancellationToken cancellationToken = default)
        {
            Project project = await _projectRepository.Find(new ProjectByIdSpecification(projectId));
            if (project == null)
                throw new DomainException($"No such project with id: {projectId}");
            Employee employee = await _employeeRepository.Find(new EmployeeByIdSpecification(employeeId));
            if (employee == null)
                throw new DomainException($"No such employee with id: {employeeId}");
            project.AddEmployee(employeeId);
            employee.AddOnProjectAsEmployee(projectId);

            await _projectRepository.UpdateAsync(project);
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task DemoteManagerToEmployee(int projectId, string reason = "", CancellationToken cancellationToken = default)
        {
            Project project = await _projectRepository.Find(new ProjectByIdSpecification(projectId));
            if (project == null)
                throw new DomainException($"No such project with id: {projectId}");
            if (project.ManagerId != null)
            {
                int managerId = (int)project.ManagerId;
                Employee employee = await _employeeRepository.Find(new EmployeeByIdSpecification(managerId));
                if (employee == null)
                    throw new DomainException($"No such employee with id: {managerId}");
                project.DemoteManagerToEmployee(reason);
                employee.RemoveFromProjectAsManager(projectId);
                employee.AddOnProjectAsEmployee(projectId);
                await _projectRepository.UpdateAsync(project);
                await _employeeRepository.UpdateAsync(employee);
            }
            else
                throw new DomainException("Project has no manager. You have to promote one of the employees to manager.");
        }

        public async Task FireManager(int projectId, string reason = "", CancellationToken cancellationToken = default)
        {
            Project project = await _projectRepository.Find(new ProjectByIdSpecification(projectId));
            if (project == null)
                throw new DomainException($"No such project with id: {projectId}");
            if (project.ManagerId != null)
            {
                int managerId = (int)project.ManagerId;
                Employee employee = await _employeeRepository.Find(new EmployeeByIdSpecification(managerId));
                if (employee == null)
                    throw new DomainException($"No such employee with id: {managerId}");
                project.FireManager(reason);
                employee.RemoveFromProjectAsManager(projectId);
                await _projectRepository.UpdateAsync(project);
                await _employeeRepository.UpdateAsync(employee);
            }
            else
                throw new DomainException("Project has no manager. You have to promote one of the employees to manager.");
        }

        public async Task PromoteEmployeeToManager(int employeeId, int projectId, CancellationToken cancellationToken = default)
        {
            Project project = await _projectRepository.Find(new ProjectByIdSpecification(projectId));
            if (project == null)
                throw new DomainException($"No such project with id: {projectId}");
            Employee employee = await _employeeRepository.Find(new EmployeeByIdSpecification(employeeId));
            if (employee == null)
                throw new DomainException($"No such employee with id: {employeeId}");
            project.PromoteEmployeeToManager(employeeId);
            employee.RemoveFromProjectAsEmployee(projectId);
            employee.AddOnProjectAsManager(projectId);
            await _projectRepository.UpdateAsync(project);
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task RemoveEmployeeFromProject(int employeeId, int projectId, CancellationToken cancellationToken = default)
        {
            Project project = await _projectRepository.Find(new ProjectByIdSpecification(projectId));
            if (project == null)
                throw new DomainException($"No such project with id: {projectId}");
            Employee employee = await _employeeRepository.Find(new EmployeeByIdSpecification(employeeId));
            if (employee == null)
                throw new DomainException($"No such employee with id: {employeeId}");
            project.RemoveEmployee(employeeId);
            employee.RemoveFromProjectAsEmployee(projectId);

            await _projectRepository.UpdateAsync(project);
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task TransferEmployeeToAnotherProject(int employeeId, int currentProjectId, int futureProjectId, CancellationToken cancellationToken = default)
        {
            Project currentProject = await _projectRepository.Find(new ProjectByIdSpecification(currentProjectId));
            if (currentProject == null)
                throw new DomainException($"No such project with id: {currentProjectId}");
            Project futureProject = await _projectRepository.Find(new ProjectByIdSpecification(futureProjectId));
            if (futureProject == null)
                throw new DomainException($"No such project with id: {futureProjectId}");
            Employee employee = await _employeeRepository.Find(new EmployeeByIdSpecification(employeeId));
            if (employee == null)
                throw new DomainException($"No such employee with id: {employeeId}");

            if (employee.Id.Equals(currentProject.ManagerId))
                throw new DomainException($"You can not transfer this employee (id: {employee.Id}) from current project" +
                $" (id: {currentProject.Id}) because he is manager of this project");

            if (!currentProject.EmployeesIds.Contains(employeeId))
                throw new DomainException($"You can not transfer this employee (id: {employeeId}) from current project" +
                $" (id: {currentProjectId}) because he is not working on it");

            if (futureProject.EmployeesIds.Contains(employeeId) || employeeId.Equals(futureProject.ManagerId))
                throw new DomainException($"You can not transfer this employee (id: {employeeId}) from current project" +
                $" (id: {currentProjectId}) because he is already works on project you want to transfer (id: {futureProjectId})");

            currentProject.RemoveEmployee(employeeId);
            employee.RemoveFromProjectAsEmployee(currentProjectId);

            futureProject.AddEmployee(employeeId);
            employee.AddOnProjectAsEmployee(futureProjectId);

            await _projectRepository.UpdateAsync(currentProject);
            await _projectRepository.UpdateAsync(futureProject);
            await _employeeRepository.UpdateAsync(employee);
        }
    }
}
