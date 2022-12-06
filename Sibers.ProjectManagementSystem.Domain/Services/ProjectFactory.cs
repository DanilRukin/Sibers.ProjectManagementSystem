using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.Services
{
    public class ProjectFactory
    {
        public Project CreateProject(string name, DateTime startDate, DateTime endDate,
            string contractorCompanyName, string customerCompanyName, int? priority = null)
        {
            return new Project(0, name,
                startDate, endDate,
                priority == null ? Priority.Default() : new Priority((int)priority),
                customerCompanyName,
                contractorCompanyName);
        }
    }
}
