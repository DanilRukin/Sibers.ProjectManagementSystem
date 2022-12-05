﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string NameOfTheCustomerCompany { get; set; } = string.Empty;
        public string NameOfTheContractorCompany { get; set; } = string.Empty;
        public List<Guid> TasksIds { get; set; }
        public List<int> EmployeesIds { get; set; }
    }
}
